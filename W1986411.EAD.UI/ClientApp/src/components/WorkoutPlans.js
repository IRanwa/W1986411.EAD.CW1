import React, { Component } from 'react';
import { withRouter } from '../withRouter';
import { Card, Button,Table } from 'antd';

class WorkoutPlan extends Component {
    constructor(props) {
        super(props);
        this.state = {
            workoutPlans: []
        }
    }
    render() {
        const workoutTbColumn = [
            { title: 'Name', dataIndex: 'name', key: 'name' },
            { title: 'Occurrence', dataIndex: 'occurrence', key: 'occurrence' },
            { title: 'Calories Burn', dataIndex: 'caloriesBurn', key: 'caloriesBurn' },
            { title: 'Operations', dataIndex: 'operations', key: 'operations' }
        ]
        return (
            <div className="p-4">
                <Card className="p-3">
                    <div>
                        <p className="form-header">Workout Plans</p>
                    </div>
                    <div>
                        <Button className="btn-custom-2">Create Plan</Button>
                    </div>
                    <div>
                        <Table columns={workoutTbColumn} dataSource={ this.state.workoutPlans}/>
                    </div>
                </Card>
            </div>
        )
    }
}

export default withRouter(WorkoutPlan);