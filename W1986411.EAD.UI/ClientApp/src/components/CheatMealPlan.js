import React, { Component } from 'react';
import { withRouter } from '../withRouter';
import { Card, Button, Table, Modal, Input, Select, DatePicker, Divider, InputNumber } from 'antd';
import OccurrenceTypes from './Enums/OccurrenceTypes';
import dayjs from 'dayjs';
import $ from 'jquery';
import { CommonGet, CommonPost } from './Utils/CommonFetch';
import { openNotification } from './Utils/CommonUI';
import NotificationStatusTypes from './Enums/NotificationStatusTypes';
import { IoTrash, IoPencil } from 'react-icons/io5';
import ScreenLoaderModal from './CommonUI/ScreenLoaderModal';

const { RangePicker } = DatePicker;

class CheatMealPlan extends Component {
    constructor(props) {
        super(props);
        this.state = {
            cheatMealPlans: [],
            modelOpen: false,
            planId: null,
            planName: "",
            occurrence: OccurrenceTypes.OneTime,
            startDateTime: new Date(),
            endDateTime: new Date(),
            cheatMealFoodName: "",
            caloriesGain: 0,

            cheatMealFoods: [],
            screenLoading: false,

            removePlanId: null,
            removeModelOpen: false
        }
    }

    componentDidMount() {
        this.getCheatMealPlans();
    }

    createCheatMealPlanModal = (status) => {
        this.setState({
            modelOpen: status,
            planId: null,
            planName: "",
            occurrence: OccurrenceTypes.OneTime,
            startDateTime: new Date(),
            endDateTime: new Date(),
            foodName: "",
            caloriesGain: 0,
            cheatMealFoods: [],
        })
    }

    setPlanName = (e) => {
        this.setState({
            planName: e.target.value
        })
        $("#planName-err").text("");
    }

    setOccurrence = (value) => {
        this.setState({
            occurrence: value
        })
    }

    setOneTimeDate = (value) => {
        this.setState({
            startDateTime: new Date(value),
            endDateTime: new Date(value)
        })
    }

    setRecurringDate = (value) => {
        this.setState({
            startDateTime: new Date(value[0]),
            endDateTime: new Date(value[1])
        })
    }

    setCheatMealFoodName = (e) => {
        this.setState({
            cheatMealFoodName: e.target.value
        })
        $("#cheatMealFoodName-err").text("");
    }

    setCaloriesGain = (value) => {
        this.setState({
            caloriesGain: value
        })
        $("#caloriesGain-err").text("");
    }

    popupCheatMealPlanRender = () => {
        const occurrenceOptions = [
            {
                value: OccurrenceTypes.OneTime,
                label: "One Time"
            },
            {
                value: OccurrenceTypes.Recurring,
                label: "Recurring"
            }
        ];
        return (
            <div className="row">
                <div className="col-md-6 mt-2">
                    <label className="form-label">Cheat Meal Plan Name</label>
                    <Input
                        name="planName"
                        placeholder="Plan Name"
                        onChange={this.setPlanName}
                        value={this.state.planName}
                        className="w-100"
                    />
                    <div id="planName-err" className="error-txt"></div>
                </div>
                <div className="col-md-6 mt-2">
                    <label className="form-label">Cheat Meal Occurrence</label>
                    <Select
                        defaultValue={this.state.occurrence}
                        options={occurrenceOptions}
                        onChange={this.setOccurrence}
                        className="w-100"
                    />
                    <div id="occurrence-err" className="error-txt"></div>
                </div>
                {
                    this.state.occurrence == OccurrenceTypes.OneTime ? (
                        <div className="col-md-6 mt-2">
                            <label className="form-label">Cheat Meal Date</label>
                            <DatePicker
                                value={dayjs(this.state.startDateTime)}
                                onChange={this.setOneTimeDate}
                                className="w-100"
                            />
                            <div id="oneTimeDate-err" className="error-txt"></div>
                        </div>
                    ) : (
                        <div className="col-md-6 mt-2">
                                <label className="form-label">Cheat Meal Date Range</label>
                            <RangePicker
                                value={[dayjs(this.state.startDateTime), dayjs(this.state.endDateTime)]}
                                onChange={this.setRecurringDate}
                                className="w-100"
                            />
                            <div id="recurringDate-err" className="error-txt"></div>
                        </div>
                    )
                }
            </div>
        )
    }

    popupCheatMealFoodsRender = () => {
        return (
            <div>
                <div className="row">
                    <div className="col-md-6 mt-2">
                        <label className="form-label">Cheat Meal Food Name</label>
                        <Input
                            name="cheatMealFoodName"
                            placeholder="Cheat Meal Food Name"
                            onChange={this.setCheatMealFoodName}
                            value={this.state.cheatMealFoodName}
                            className="w-100"
                        />
                        <div id="cheatMealFoodName-err" className="error-txt"></div>
                    </div>
                    <div className="col-md-6 mt-2">
                        <label className="form-label">Calories Gain</label>
                        <InputNumber
                            name="caloriesGain"
                            placeholder="Calories Gain"
                            onChange={this.setCaloriesGain}
                            value={this.state.caloriesGain}
                            className="w-100"
                        />
                        <div id="caloriesGain-err" className="error-txt"></div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12" align="right">
                        <Button className="btn-custom-2" onClick={this.addCheatMealFood}>Add</Button>
                    </div>
                </div>
            </div>
        )
    }

    popupCreateModal = () => {
        const cheatMealFoodTbColumn = [
            { title: 'Name', dataIndex: 'name', key: 'name' },
            { title: 'Calories Gain', dataIndex: 'caloriesGain', key: 'caloriesGain' },
            { title: 'Operations', dataIndex: 'operations', key: 'operations' },
        ];
        let title = this.state.planId == null ? "Create Cheat Meal Plan" : "Update Cheat Meal Plan";
        let footerBtns = [
            <Button className="btn-cancel-custom-2" onClick={() => this.createCheatMealPlanModal(false)}>Cancel</Button>
        ];
        this.state.planId == null ?
            footerBtns.push(<Button className="btn-custom-2" onClick={this.submitCheatMealPlan}>Submit</Button>) :
            footerBtns.push(<Button className="btn-custom-2" onClick={this.updateCheatMealPlan}>Submit</Button>);
        return (
            <Modal
                title={title}
                open={this.state.modelOpen}
                width={"40%"}
                onCancel={() => this.createCheatMealPlanModal(false)}
                footer={footerBtns}
            >
                {this.popupCheatMealPlanRender()}
                <Divider />
                {this.popupCheatMealFoodsRender()}
                <div>
                    <Table columns={cheatMealFoodTbColumn} dataSource={[...this.state.cheatMealFoods]} />
                </div>
            </Modal>
        )
    }

    addCheatMealFood = () => {
        let isValid = this.validateCheatMealFoodFields();
        if (!isValid) {
            return;
        }
        let cheatMealFoods = this.state.cheatMealFoods;
        let key = cheatMealFoods.length == 0 ? 1 : cheatMealFoods[cheatMealFoods.length - 1].key+1;
        let operations = (
            <div >
                <IoTrash className="color-red cursor-pointer operation-icon" onClick={() => this.removeCheatMealFood(key)} />
            </div>
        );
        cheatMealFoods.push({
            key: key,
            name: this.state.cheatMealFoodName,
            caloriesGain: this.state.caloriesGain,
            operations: operations
        });
        this.setState({
            cheatMealFoods: cheatMealFoods,
            cheatMealFoodName: "",
            caloriesGain: 0,
        })
    }

    removeCheatMealFood = (key) => {
        let cheatMealFoods = this.state.cheatMealFoods;
        let index = cheatMealFoods.indexOf(cheatMealFoods.find(x => x.key == key));
        cheatMealFoods.splice(index, 1);
        this.setState({
            cheatMealFoods: cheatMealFoods
        })
    }

    submitCheatMealPlan = () => {
        let isValid = this.validateCheatMealPlan();
        if (!isValid) {
            return;
        }
        let cheatMealFoods = [];
        for (let food of this.state.cheatMealFoods) {
            cheatMealFoods.push({
                "name": food.name,
                "caloriesGain": food.caloriesGain,
                "isActive": true
            })
        }
        let data = {
            "cheatMealName": this.state.planName,
            "occurrenceType": this.state.occurrence,
            "startDate": this.state.startDateTime,
            "endDate": this.state.endDateTime,
            "isActive": true,
            "foods": cheatMealFoods
        }
        this.setState({
            screenLoading: true
        })
        CommonPost("/api/v1/cheatmeal", null, data)
            .then(res => {
                if (res.isSuccess) {
                    openNotification(NotificationStatusTypes.Success, res.message);
                    this.createCheatMealPlanModal(false);
                    this.getCheatMealPlans();
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Cheat meal plan creation error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    getCheatMealPlans = () => {
        this.setState({
            screenLoading: true
        })
        CommonGet("/api/v1/cheatmeal", null)
            .then(res => {
                if (res.isSuccess) {
                    let plans = res.data;
                    let cheatMealPlans = [];
                    for (let plan of plans) {
                        const operations = (
                            <div >
                                <span className="p-2">
                                    <IoPencil className="color-blue cursor-pointer operation-icon" onClick={() => this.editCheatMealPlan(plan.id)} />
                                </span>
                                <span className="p-2">
                                    <IoTrash className="color-red cursor-pointer operation-icon" onClick={() => this.cheatMealPlanRemoveModel(plan.id, true)} />
                                </span>
                            </div>
                        );
                        cheatMealPlans.push({
                            planId: plan.id,
                            name: plan.name,
                            occurrence: plan.occurrenceType,
                            caloriesGain: plan.caloriesGain,
                            startDate: new Date(plan.startDate).toDateString(),
                            endDate: new Date(plan.endDate).toDateString(),
                            operations: operations
                        })
                    }
                    this.setState({
                        cheatMealPlans
                    })
                }
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Cheat meal plans retrieve error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    validateCheatMealFoodFields = () => {
        let isValid = true;
        if (this.state.cheatMealFoodName === "") {
            $("#cheatMealFoodName-err").text("Cheat meal food name required");
            isValid = false;
        }
        if (this.state.caloriesGain === 0) {
            $("#caloriesGain-err").text("Calories gain required");
            isValid = false;
        }
        return isValid;
    }

    validateCheatMealPlan = () => {
        let isValid = true;
        if (this.state.planName === "") {
            $("#planName-err").text("Cheat meal plan name required");
            isValid = false;
        }
        if (this.state.cheatMealFoods.length == 0) {
            openNotification(NotificationStatusTypes.Warning, "No cheat meal food items recorded");
            isValid = false;
        }
        return isValid;
    }

    rendeRemoveModel = () => {
        let footerBtns = [
            <Button className="btn-cancel-custom-2" onClick={() => this.cheatMealPlanRemoveModel(null, false)}>Cancel</Button>,
            <Button className="btn-custom-2" onClick={() => this.removeCheatMealPlan()}>Remove</Button>,
        ];
        return (
            <Modal
                title="Remove Cheat Meal Plan"
                open={this.state.removeModelOpen}
                width={"40%"}
                footer={footerBtns}
            >
                <span>Are you sure you want to remove the cheat meal plan?</span>
            </Modal>
        )
    }

    cheatMealPlanRemoveModel = (planId, status) => {
        this.setState({
            removePlanId: planId,
            removeModelOpen: status
        })
    }

    removeCheatMealPlan = () => {
        this.setState({
            screenLoading: true
        });
        CommonPost("/api/v1/cheatmeal/remove/" + this.state.removePlanId, null)
            .then(res => {
                if (res.isSuccess) {
                    openNotification(NotificationStatusTypes.Success, res.message);
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.cheatMealPlanRemoveModel(null, false);
                this.getCheatMealPlans();
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Cheat meal plan remove error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    editCheatMealPlan = (planId) => {
        this.setState({
            screenLoading: true
        });
        CommonGet("/api/v1/cheatmeal/" + planId, null)
            .then(res => {
                if (res.isSuccess) {
                    let data = res.data;
                    console.log("data ", data)
                    let cheatMealFoods = [];
                    for (let food of data.cheatMealPlanFoods) {
                        let key = cheatMealFoods.length == 0 ? 1 : cheatMealFoods[cheatMealFoods.length - 1].key +1;
                        let operations = (
                            <div >
                                <IoTrash className="color-red cursor-pointer operation-icon" onClick={() => this.removeCheatMealFood(key)} />
                            </div>
                        );
                        cheatMealFoods.push({
                            key: key,
                            name: food.name,
                            caloriesGain: food.caloriesGain,
                            operations: operations
                        });
                    }
                    this.setState({
                        planId: data.id,
                        planName: data.cheatMealName,
                        occurrence: data.occurrenceType,
                        startDateTime: data.startDate,
                        endDateTime: data.endDate,
                        cheatMealFoods: cheatMealFoods,
                        modelOpen: true
                    })
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Cheat meal plan details retrieve error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    updateCheatMealPlan = () => {
        let isValid = this.validateCheatMealPlan();
        if (!isValid) {
            return;
        }
        let cheatMealFoods = [];
        for (let food of this.state.cheatMealFoods) {
            cheatMealFoods.push({
                "name": food.name,
                "caloriesGain": food.caloriesGain,
                "isActive": true
            })
        }
        let data = {
            "Id": this.state.planId,
            "cheatMealName": this.state.planName,
            "occurrenceType": this.state.occurrence,
            "startDate": this.state.startDateTime,
            "endDate": this.state.endDateTime,
            "isActive": true,
            "foods": cheatMealFoods
        }
        this.setState({
            screenLoading: true
        })
        CommonPost("/api/v1/cheatmeal/update", null, data)
            .then(res => {
                if (res.isSuccess) {
                    openNotification(NotificationStatusTypes.Success, res.message);
                    this.createCheatMealPlanModal(false);
                    this.getCheatMealPlans();
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Cheat meal plan update error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    render() {
        const cheatMealTbColumn = [
            { title: 'Name', dataIndex: 'name', key: 'name' },
            { title: 'Occurrence', dataIndex: 'occurrence', key: 'occurrence' },
            { title: 'Start Date', dataIndex: 'startDate', key: 'startDate' },
            { title: 'End Date', dataIndex: 'endDate', key: 'endDate' },
            { title: 'Calories Gain', dataIndex: 'caloriesGain', key: 'caloriesGain' },
            { title: 'Operations', dataIndex: 'operations', key: 'operations' }
        ];
        return (
            <ScreenLoaderModal loading={this.state.screenLoading}>
                {
                    <div className="p-4">
                        <Card className="p-3">
                            <div>
                                <p className="form-header">Cheat Meal Plans</p>
                            </div>
                            <div>
                                <Button className="btn-custom-2" onClick={() => this.createCheatMealPlanModal(true)}>Create Plan</Button>
                            </div>
                            <div>
                                <Table columns={cheatMealTbColumn} dataSource={[...this.state.cheatMealPlans]} />
                            </div>
                        </Card>
                        {
                            this.state.modelOpen ? (
                                this.popupCreateModal()
                            ) : ("")
                        }
                        {
                            this.state.removeModelOpen ? (
                                this.rendeRemoveModel()
                            ) : ("")
                        }
                    </div>
                }
            </ScreenLoaderModal>
        )
    }
}

export default withRouter(CheatMealPlan);