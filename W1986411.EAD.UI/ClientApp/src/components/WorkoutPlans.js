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

class WorkoutPlan extends Component {
    constructor(props) {
        super(props);
        this.state = {
            workoutPlans: [],
            modelOpen: false,
            planId: null,
            planName: "",
            occurrence: OccurrenceTypes.OneTime,
            startDateTime: new Date(),
            endDateTime: new Date(),
            workoutName: "",
            workoutSets: 0,
            workoutReps: 0,
            caloriesBurn: 0,

            workoutRoutines: [],
            screenLoading: false,

            removePlanId: null,
            removeModelOpen: false
        }
    }

    componentDidMount() {
        this.getWorkoutPlans();
    }

    createWorkoutPlanModal = (status) => {
        this.setState({
            modelOpen: status,
            planId: null,
            planName: "",
            occurrence: OccurrenceTypes.OneTime,
            startDateTime: new Date(),
            endDateTime: new Date(),
            workoutName: "",
            workoutSets: 0,
            workoutReps: 0,
            caloriesBurn: 0,
            workoutRoutines: []
        })
    }

    setPlanName = (e) => {
        this.setState({
            planName: e.target.value
        })
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

    setWorkoutName = (e) => {
        this.setState({
            workoutName: e.target.value
        })
    }

    setWorkoutSets = (value) => {
        this.setState({
            workoutSets: value
        })
    }

    setWorkoutReps = (value) => {
        this.setState({
            workoutReps: value
        })
    }

    setCaloriesBurn = (value) => {
        this.setState({
            caloriesBurn: value
        })
    }

    popupWorkoutPlanRender = () => {
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
                    <label className="form-label">Workout Plan Name</label>
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
                    <label className="form-label">Workout Occurrence</label>
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
                            <label className="form-label">Workout Date</label>
                            <DatePicker
                                value={dayjs(this.state.startDateTime)}
                                onChange={this.setOneTimeDate}
                                className="w-100"
                            />
                            <div id="oneTimeDate-err" className="error-txt"></div>
                        </div>
                    ) : (
                        <div className="col-md-6 mt-2">
                            <label className="form-label">Workout Date Range</label>
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

    popupWorkoutRoutineRender = () => {
        return (
            <div>
                <div className="row">
                    <div className="col-md-6 mt-2">
                        <label className="form-label">Workout Name</label>
                        <Input
                            name="workoutName"
                            placeholder="Workout Name"
                            onChange={this.setWorkoutName}
                            value={this.state.workoutName}
                            className="w-100"
                        />
                        <div id="workoutName-err" className="error-txt"></div>
                    </div>
                    <div className="col-md-6 mt-2">
                        <label className="form-label">Workout Sets</label>
                        <InputNumber
                            name="workoutSets"
                            placeholder="Workout Sets"
                            onChange={this.setWorkoutSets}
                            value={this.state.workoutSets}
                            className="w-100"
                            max={150}
                        />
                        <div id="workoutSets-err" className="error-txt"></div>
                    </div>
                    <div className="col-md-6 mt-2">
                        <label className="form-label">Workout Reps</label>
                        <InputNumber
                            name="workoutReps"
                            placeholder="Workout Reps"
                            onChange={this.setWorkoutReps}
                            value={this.state.workoutReps}
                            className="w-100"
                            max={150}
                        />
                        <div id="workoutReps-err" className="error-txt"></div>
                    </div>
                    <div className="col-md-6 mt-2">
                        <label className="form-label">Calories Burn</label>
                        <InputNumber
                            name="caloriesBurn"
                            placeholder="Calories Burn"
                            onChange={this.setCaloriesBurn}
                            value={this.state.caloriesBurn}
                            className="w-100"
                        />
                        <div id="caloriesBurn-err" className="error-txt"></div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12" align="right">
                        <Button className="btn-custom-2" onClick={this.addWorkoutRoutine}>Add</Button>
                    </div>
                </div>
            </div>
        )
    }

    popupCreateModal = () => {
        const workoutRoutineTbColumn = [
            { title: 'Name', dataIndex: 'name', key: 'name' },
            { title: 'Sets', dataIndex: 'sets', key: 'sets' },
            { title: 'Reps', dataIndex: 'reps', key: 'reps' },
            { title: 'Calories Burn', dataIndex: 'caloriesBurn', key: 'caloriesBurn' },
            { title: 'Operations', dataIndex: 'operations', key: 'operations' },
        ];
        let title = this.state.planId == null ? "Create Workout Plan" : "Update Workout Plan";
        let footerBtns = [
            <Button key="1" className="btn-cancel-custom-2" onClick={() => this.createWorkoutPlanModal(false)}>Cancel</Button>
        ];
        this.state.planId == null ?
            footerBtns.push(<Button key="2" className="btn-custom-2" onClick={this.submitWorkoutPlan}>Submit</Button>) :
            footerBtns.push(<Button key="3" className="btn-custom-2" onClick={this.updateWorkoutPlan}>Submit</Button>);
        return (
            <Modal
                title={title}
                open={this.state.modelOpen}
                width={"40%"}
                onCancel={() => this.createWorkoutPlanModal(false)}
                footer={footerBtns}
            >
                {this.popupWorkoutPlanRender()}
                <Divider />
                {this.popupWorkoutRoutineRender()}
                <div>
                    <Table columns={workoutRoutineTbColumn} dataSource={[...this.state.workoutRoutines]} />
                </div>
            </Modal>
        )
    }

    addWorkoutRoutine = () => {
        let isValid = this.validateWorkoutRoutineFields();
        if (!isValid) {
            return;
        }
        let routines = this.state.workoutRoutines;
        let key = routines.length == 0 ? 1 : routines[routines.length - 1].key;
        let operations = (
            <div >
                <IoTrash className="color-red cursor-pointer operation-icon" onClick={() => this.removeWorkoutRoutine(key)} />
            </div>
        );
        routines.push({
            key: key,
            name: this.state.workoutName,
            sets: this.state.workoutSets,
            reps: this.state.workoutReps,
            caloriesBurn: this.state.caloriesBurn,
            operations: operations
        });
        this.setState({
            workoutRoutines: routines,
            workoutName: "",
            workoutSets: 0,
            workoutReps: 0,
            caloriesBurn: 0,
        })
    }

    removeWorkoutRoutine = (key) => {
        let routines = this.state.workoutRoutines;
        let index = routines.indexOf(routines.find(x => x.key == key));
        routines.splice(index, 1);
        this.setState({
            workoutRoutines: routines
        })
    }

    submitWorkoutPlan = () => {
        let isValid = this.validateWorkoutPlan();
        if (!isValid) {
            return;
        }
        let routines = [];
        for (let routine of this.state.workoutRoutines) {
            routines.push({
                "name": routine.name,
                "sets": routine.sets,
                "reps": routine.reps,
                "burnCalories": routine.caloriesBurn,
                "isActive": true
            })
        }
        let data = {
            "workoutName": this.state.planName,
            "occurrenceType": this.state.occurrence,
            "startDate": this.state.startDateTime,
            "endDate": this.state.endDateTime,
            "isActive": true,
            "routines": routines
        }
        this.setState({
            screenLoading: true
        })
        CommonPost("/api/v1/workout", null, data)
            .then(res => {
                if (res.isSuccess) {
                    openNotification(NotificationStatusTypes.Success, res.message);
                    this.createWorkoutPlanModal(false);
                    this.getWorkoutPlans();
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Workout plan creation error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    getWorkoutPlans = () => {
        this.setState({
            screenLoading: true
        })
        CommonGet("/api/v1/workout", null)
            .then(res => {
                if (res.isSuccess) {
                    let plans = res.data;
                    let workoutPlans = [];
                    for (let plan of plans) {
                        const operations = (
                            <div >
                                <span className="p-2">
                                    <IoPencil className="color-blue cursor-pointer operation-icon" onClick={() => this.editWorkoutPlan(plan.planId)} />
                                </span>
                                <span className="p-2">
                                    <IoTrash className="color-red cursor-pointer operation-icon" onClick={() => this.workoutPlanRemoveModel(plan.planId, true)} />
                                </span>
                            </div>
                        );
                        workoutPlans.push({
                            planId: plan.planId,
                            name: plan.planName,
                            occurrence: plan.occurrenceType,
                            caloriesBurn: plan.caloriesBurn,
                            operations: operations
                        })
                    }
                    this.setState({
                        workoutPlans
                    })
                }
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Workout plans retrieve error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    validateWorkoutRoutineFields = () => {
        let isValid = true;
        if (this.state.workoutName === "") {
            $("#workoutName-err").text("Workout name required");
            isValid = false;
        }
        if (this.state.workoutSets === 0) {
            $("#workoutSets-err").text("Workout sets required");
            isValid = false;
        }
        if (this.state.workoutReps === 0) {
            $("#workoutReps-err").text("Workout reps required");
            isValid = false;
        }
        if (this.state.caloriesBurn === 0) {
            $("#caloriesBurn-err").text("Calories burn required");
            isValid = false;
        }
        return isValid;
    }

    validateWorkoutPlan = () => {
        let isValid = true;
        if (this.state.planName === "") {
            $("#planName-err").text("Workout plan name required");
            isValid = false;
        }
        if (this.state.workoutRoutines.length == 0) {
            openNotification(NotificationStatusTypes.Warning, "No workout routine recorded");
            isValid = false;
        }
        return isValid;
    }

    rendeRemoveModel = () => {
        let footerBtns = [
            <Button key="1" className="btn-cancel-custom-2" onClick={() => this.workoutPlanRemoveModel(null, false)}>Cancel</Button>,
            <Button key="2" className="btn-custom-2" onClick={() => this.removeWorkoutPlan()}>Remove</Button>,
        ];
        return (
            <Modal
                title="Remove Workout Plan"
                open={this.state.removeModelOpen}
                width={"40%"}
                footer={footerBtns }
            >
                <span>Are you sure you want to remove the workout plan?</span>
            </Modal>
        )
    }

    workoutPlanRemoveModel = (planId, status) => {
        this.setState({
            removePlanId: planId,
            removeModelOpen: status
        })
    }

    removeWorkoutPlan = () => {
        this.setState({
            screenLoading: true
        });
        CommonPost("/api/v1/workout/remove/" + this.state.removePlanId, null)
        .then(res => {
            if (res.isSuccess) {
                openNotification(NotificationStatusTypes.Success, res.message);
            } else {
                openNotification(NotificationStatusTypes.Error, res.message);
            }
            this.workoutPlanRemoveModel(null, false);
            this.getWorkoutPlans();
            this.setState({
                screenLoading: false
            })
        }).catch(err => {
            openNotification(NotificationStatusTypes.Error, "Workout plan remove error occurred.");
            this.setState({
                screenLoading: false
            })
        })
    }

    editWorkoutPlan = (planId) => {
        this.setState({
            screenLoading: true
        });
        CommonGet("/api/v1/workout/" + planId, null)
            .then(res => {
                if (res.isSuccess) {
                    let data = res.data;
                    let routines = [];
                    for (let routine of data.workoutPlanRoutines) {
                        let key = routines.length == 0 ? 1 : routines[routines.length - 1].key;
                        let operations = (
                            <div >
                                <IoTrash className="color-red cursor-pointer operation-icon" onClick={() => this.removeWorkoutRoutine(key)} />
                            </div>
                        );
                        routines.push({
                            key: key,
                            name: routine.name,
                            sets: routine.sets,
                            reps: routine.reps,
                            caloriesBurn: routine.burnCalories,
                            operations: operations
                        });
                    }
                    this.setState({
                        planId: data.id,
                        planName: data.workoutName,
                        occurrence: data.occurrenceType,
                        startDateTime: data.startDate,
                        endDateTime: data.endDate,
                        workoutRoutines: routines,
                        modelOpen: true
                    })
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Workout plan details retrieve error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    updateWorkoutPlan = () => {
        let isValid = this.validateWorkoutPlan();
        if (!isValid) {
            return;
        }
        let routines = [];
        for (let routine of this.state.workoutRoutines) {
            routines.push({
                "name": routine.name,
                "sets": routine.sets,
                "reps": routine.reps,
                "burnCalories": routine.caloriesBurn,
                "isActive": true
            })
        }
        let data = {
            "Id": this.state.planId, 
            "workoutName": this.state.planName,
            "occurrenceType": this.state.occurrence,
            "startDate": this.state.startDateTime,
            "endDate": this.state.endDateTime,
            "isActive": true,
            "routines": routines
        }
        this.setState({
            screenLoading: true
        })
        CommonPost("/api/v1/workout/update", null, data)
            .then(res => {
                if (res.isSuccess) {
                    openNotification(NotificationStatusTypes.Success, res.message);
                    this.createWorkoutPlanModal(false);
                    this.getWorkoutPlans();
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.setState({
                    screenLoading: false
                })
            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "Workout plan update error occurred.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    render() {
        const workoutTbColumn = [
            { title: 'Name', dataIndex: 'name', key: 'name' },
            { title: 'Occurrence', dataIndex: 'occurrence', key: 'occurrence' },
            { title: 'Calories Burn', dataIndex: 'caloriesBurn', key: 'caloriesBurn' },
            { title: 'Operations', dataIndex: 'operations', key: 'operations' }
        ];
        return (
            <ScreenLoaderModal loading={this.state.screenLoading}>
                {
                    <div className="p-4">
                        <Card className="p-3">
                            <div>
                                <p className="form-header">Workout Plans</p>
                            </div>
                            <div>
                                <Button className="btn-custom-2" onClick={() => this.createWorkoutPlanModal(true)}>Create Plan</Button>
                            </div>
                            <div>
                                <Table columns={workoutTbColumn} dataSource={[...this.state.workoutPlans]} />
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
                            ):("")
                        }
                    </div>
                }
            </ScreenLoaderModal>
        )
    }
}

export default withRouter(WorkoutPlan);