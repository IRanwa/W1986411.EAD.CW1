import React, { Component } from "react";
import { withRouter } from "../../withRouter";


class Unauthorize extends Component {
    constructor(props) {
        super(props);
        this.state = {
            allowAccess: false
        }
    }

    componentDidMount() {
        var token = localStorage.getItem("token");
        if (token == null || token == undefined || token == "null") {
            window.location.href = "/";
            //openNotification(NotificationStatusTypes.Error, "", "Unauthorize access.")
        } else {
            this.setState({
                allowAccess: true
            })
        }
    }

    render() {
        return (
            <div>
                {
                    this.state.allowAccess ? (
                        this.props.children
                    ) : ("")
                }

            </div>
        )
    }
}

export default withRouter(Unauthorize);