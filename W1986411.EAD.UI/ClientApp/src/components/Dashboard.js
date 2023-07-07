import React, { Component } from 'react';
import ScreenLoaderModal from './CommonUI/ScreenLoaderModal';
import { Calendar, Card, Button, Modal, DatePicker, Divider, Input, Select, Badge } from 'antd';
import dayjs from 'dayjs';
import { CommonGet, CommonPost } from './Utils/CommonFetch';
import FitnessStatusTypes from './Enums/FitnessStatusTypes';
import { openNotification } from './Utils/CommonUI';
import NotificationStatusTypes from './Enums/NotificationStatusTypes';
import { CSVLink } from "react-csv";

class Dashboard extends Component {
    constructor(props) {
        super(props);
        this.state = {
            screenLoading: false,
            fitnessModelOpen: false,
            fitnessSelectedDate: new Date(),
            fitnessId: null,
            fitnessWeight: 0,
            fitnessStatus: FitnessStatusTypes.Fair,

            calendarFitnessData: [],
            calendarSelectedDate: new Date()
        }
    }

    componentDidMount() {
        this.getFitnessDetailsForPeriod();
    }

    onPanelChange = (value) => {
        this.setState({
            calendarSelectedDate: value
        }, () => {
            this.getFitnessDetailsForPeriod();
        })
    }

    getFitnessDetailsForPeriod = () => {
        this.setState({
            screenLoading: true
        });
        CommonPost("/api/v1/user-fitness/by-period", null, { recordDate: this.state.calendarSelectedDate })
            .then(res => {
                if (res.isSuccess) {
                    this.setState({
                        calendarFitnessData: res.data
                    })
                }
                this.setState({
                    screenLoading: false
                });
            }).catch(err => {
                this.setState({
                    screenLoading: false
                });
            })
    }

    fitnessDetailsModel = (status) => {
        this.setState({
            fitnessModelOpen: status
        }, () => {
            this.getFitnessDetailsByDate();
        })
    }

    dateCellRender = (current) =>{
        var currentDate = new Date(current);
        var calendarFitnessData = this.state.calendarFitnessData;
        var fitnessData = calendarFitnessData.find(data =>
            new Date(data.recordDate).getFullYear().toString() == currentDate.getFullYear().toString() &&
            new Date(data.recordDate).getMonth().toString() == currentDate.getMonth().toString() &&
            new Date(data.recordDate).getDate().toString() == currentDate.getDate().toString());
        if (fitnessData != undefined) {
            return (
                <div align="left">
                    {
                        fitnessData.caloriesBurn != "0" ? (
                            <div>
                                <Badge className="form-label" status="success" text={"Calories Burn : " + fitnessData.caloriesBurn} />
                            </div>
                        ) : ("")
                    }
                    {
                        fitnessData.predCaloriesBurn != "0" ? (
                            <div>
                                <Badge className="form-label" status="processing" text={"Calories Burn : " + fitnessData.predCaloriesBurn} />
                            </div>
                        ) : ("")
                    }

                    {
                        fitnessData.caloriesGain != "0" ? (
                            <div>
                                <Badge className="form-label" status="success" text={"Calories Gain  : " + fitnessData.caloriesGain} />
                            </div>
                        ) : ("")
                    }
                    {
                        fitnessData.predCaloriesGain != "0" ? (
                            <div>
                                <Badge className="form-label" status="processing" text={"Calories Gain  : " + fitnessData.predCaloriesGain} />
                            </div>
                        ) : ("")
                    }

                    {
                        fitnessData.weight != "0" ? (
                            <div>
                                <Badge className="form-label" status="success" text={"Weight : " + fitnessData.weight + " kg"} />
                            </div>
                        ):("")
                    }
                    {
                        fitnessData.predWeight != "0" ? (
                            <div>
                                <Badge className="form-label" status="processing" text={"Weight : " + fitnessData.predWeight + " kg"} />
                            </div>
                        ) : ("")
                    }

                    {
                        fitnessData.fitnessStatusStr != "" && fitnessData.fitnessStatusStr  != null? (
                            <div>
                                <Badge className="form-label" status="success" text={"Fitness Status : " + fitnessData.fitnessStatusStr} />
                            </div>
                        ) : ("")
                    }
                    {
                        fitnessData.predFitnessStatusStr != "" && fitnessData.predFitnessStatusStr != null ? (
                            <div>
                                <Badge className="form-label" status="processing" text={"Fitness Status : " + fitnessData.predFitnessStatusStr} />
                            </div>
                        ) : ("")
                    }

                    
                </div>
            )
        }
    }

    cellRender = (current, info) => {
        if (info.type === 'date') return this.dateCellRender(current);
    }

    getFitnessDetailsByDate = () => {
        this.setState({
            screenLoading: true
        });
        CommonPost("/api/v1/user-fitness/by-date", null, { recordDate: this.state.fitnessSelectedDate })
            .then(res => {
                console.log(res)
                if (res.isSuccess) {
                    let data = res.data;
                    this.setState({
                        fitnessId: data.id,
                        fitnessWeight: data.weight,
                        fitnessStatus: data.fitnessStatus
                    });
                } else {
                    this.setState({
                        fitnessId: null,
                        fitnessWeight: 0,
                        fitnessStatus: FitnessStatusTypes.Fair
                    });
                }
                this.setState({
                    screenLoading: false
                });
            }).catch(err => {
                this.setState({
                    screenLoading: false
                });
            })
    }

    setFitnessDetailDate = (value) => {
        this.setState({
            fitnessSelectedDate: value
        }, () => {
            this.getFitnessDetailsByDate();
        })
    }

    setWeight = (e) => {
        this.setState({
            fitnessWeight: e.target.value
        })
    }

    setFitnessStatus = (value) => {
        this.setState({
            fitnessStatus: value
        })
    }

    addUpdateFitnessDetails = () => {
        this.setState({
            screenLoading: true
        });
        let data = {
            weight: this.state.fitnessWeight,
            fitnessStatus: this.state.fitnessStatus,
            recordDate: this.state.fitnessSelectedDate,
            isActive: true
        };
        CommonPost("/api/v1/user-fitness", null, data)
            .then(res => {
                console.log(res)
                if (res.isSuccess) {
                    openNotification(NotificationStatusTypes.Success, res.message);
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.setState({
                    screenLoading: false,
                    fitnessModelOpen: false,
                    fitnessSelectedDate: new Date(),
                    fitnessId: null,
                    fitnessWeight: 0,
                    fitnessStatus: FitnessStatusTypes.Fair
                });
                this.getFitnessDetailsForPeriod();
            }).catch(err => {
                this.setState({
                    screenLoading: false
                });
                openNotification(NotificationStatusTypes.Error, "Fitness details update error occurred.");
            })
    }

    removeFitnessDetail = () => {
        this.setState({
            screenLoading: true
        });
        let data = {
            recordDate: this.state.fitnessSelectedDate,
        };
        CommonPost("/api/v1/user-fitness/delete", null, data)
            .then(res => {
                console.log("res ", res)
                if (res.isSuccess) {
                    openNotification(NotificationStatusTypes.Success, res.message);
                } else {
                    openNotification(NotificationStatusTypes.Error, res.message);
                }
                this.setState({
                    screenLoading: false,
                    fitnessModelOpen: false,
                    fitnessSelectedDate: new Date(),
                    fitnessId: null,
                    fitnessWeight: 0,
                    fitnessStatus: FitnessStatusTypes.Fair
                });
                this.getFitnessDetailsForPeriod();
            }).catch(err => {
                console.log("err ",err)
                this.setState({
                    screenLoading: false
                });
                openNotification(NotificationStatusTypes.Error, "Fitness details remove error occurred.");
            })
    }

    renderFitnessDetailsPopupModal = () => {
        let footerBtns = [
            <Button key="1" className="btn-cancel-custom-2" onClick={() => this.fitnessDetailsModel(false)}>Cancel</Button>
        ];
        if (this.state.fitnessId > 0) {
            footerBtns.push(<Button key="2" className="btn-custom-3" onClick={() => this.addUpdateFitnessDetails()}>Edit</Button>);
            footerBtns.push(<Button key="3" className="btn-custom-2" onClick={() => this.removeFitnessDetail()}>Remove</Button>);
        } else {
            footerBtns.push(<Button key="4" className="btn-custom-2" onClick={() => this.addUpdateFitnessDetails()}>Add</Button>);
        }

        let fitnessStatusOptions = [
            {
                value: FitnessStatusTypes.VeryPoor,
                label: "Very Poor"
            },
            {
                value: FitnessStatusTypes.Poor,
                label: "Poor"
            },
            {
                value: FitnessStatusTypes.Fair,
                label: "Fair"
            },
            {
                value: FitnessStatusTypes.Good,
                label: "Good"
            },
            {
                value: FitnessStatusTypes.Excellent,
                label: "Excellent"
            },
            {
                value: FitnessStatusTypes.Superior,
                label: "Superior"
            }
        ];
        return (
            <Modal
                title="Update Fitness Detail"
                open={this.state.fitnessModelOpen}
                width={"40%"}
                onCancel={() => this.fitnessDetailsModel(false)}
                footer={footerBtns}
            >
                <div>
                    <DatePicker
                        value={dayjs(this.state.fitnessSelectedDate)}
                        onChange={this.setFitnessDetailDate}
                        className="w-100"
                        disabledDate={d => !d || d.isAfter(new Date())}
                    />
                </div>
                <Divider />
                <div className="row">
                    <div className="col-md-6 mt-2">
                        <label className="form-label">Weight (Kg)</label>
                        <Input
                            name="weight"
                            placeholder="50"
                            onChange={this.setWeight}
                            value={this.state.fitnessWeight}
                            className="w-100"
                        />
                        <div id="weight-err" className="error-txt"></div>
                    </div>
                    <div className="col-md-6 mt-2">
                        <label className="form-label">Fitness Status</label>
                        <Select
                            value={this.state.fitnessStatus}
                            options={fitnessStatusOptions}
                            onChange={this.setFitnessStatus}
                            className="w-100"
                        />
                        <div id="fitnessStatus-err" className="error-txt"></div>
                    </div>
                </div>
            </Modal>
        )
    }

    updateFitnessDetail = () => {

    }

    render() {
        const fileHeaders = [
            { label: '#', key: 'id' },
            { label: 'Record date', key: 'recordDate' },
            { label: 'Fitness Status', key: 'fitnessStatusStr' },
            { label: 'Weight', key: 'weight' },
            { label: 'Calories Burn', key: 'caloriesGain' },
            { label: 'Calories Gain', key: 'caloriesBurn' },
            { label: 'Fitness Status (Prediction)', key: 'predFitnessStatusStr' },
            { label: 'Weight (Prediction)', key: 'predWeight' },
            { label: 'Calories Burn (Prediction)', key: 'predCaloriesGain' },
            { label: 'Calories Gain (Prediction)', key: 'predCaloriesBurn' }
        ];
        return (
            <ScreenLoaderModal loading={this.state.screenLoading}>
                {
                    <div className="p-4">
                        <Card className="p-3" >
                            <div>
                                <p className="form-header">Dashboard</p>
                            </div>
                            <div align="right">
                                <Button className="btn-custom-2" onClick={() => this.fitnessDetailsModel(true)}>Update Fitness Details</Button>
                            </div>
                            <div>
                                <div>
                                    <span className="m-2"><Badge className="form-label" status="success" text={"Actual"} /></span>
                                    <span className="m-2"><Badge className="form-label" status="processing" text={"Prediction"} /></span>
                                </div>
                                <CSVLink
                                    headers={fileHeaders}
                                    filename={"Dashboard.csv"}
                                    data={this.state.calendarFitnessData}
                                    className="btn btn-custom-2"
                                >
                                    Export to CSV
                                </CSVLink>
                                <Calendar className="dashboard-calendar" fullscreen={false} onPanelChange={this.onPanelChange} cellRender={this.cellRender} />;
                            </div>
                        </Card>
                    </div>
                }
                {
                    this.state.fitnessModelOpen ? (
                        this.renderFitnessDetailsPopupModal()
                    ) : ("")
                }
            </ScreenLoaderModal>
        )
    }
}

export default Dashboard;