import React, { Component } from 'react';
import $ from 'jquery';
import { Input, Card, Button, Divider } from 'antd';
import { UserOutlined, KeyOutlined } from '@ant-design/icons';
import ScreenLoaderModal from './CommonUI/ScreenLoaderModal';
import { CommonGet, CommonPost } from './Utils/CommonFetch';
import { openNotification } from './Utils/CommonUI';
import NotificationStatusTypes from './Enums/NotificationStatusTypes';
import { withRouter } from '../withRouter';

class LoginRegistration extends Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            confirmPassword: '',
            firstName: '',
            lastName: '',
            screenLoading: false,
            hideUI: true,

            loginScreenEnabled: true,
            regScreenEnabled: false,
            passwordResetScreenEnabled: false
        }

    }

    checkRedirections = () => {
        var token = localStorage.getItem("token");
       
        if (token !== undefined && token !== null && token !== "null") {
            window.location.href = "/dashboard";
        }
        setTimeout(() => {
            this.setState({
                hideUI: false
            })
        }, 500)

    }

    componentDidMount() {   
        this.checkRedirections();
    }

    onChangeEmail = (e) => {
        let value = e.target.value;
        this.setState({
            email: value
        }, () => {
            if (value === "") {
                $("#login-email-err").text("Email address required");
            } else {
                $("#login-email-err").text("");
            }
        });
    }

    onPasswordChange = (e) => {
        let value = e.target.value;
        this.setState({
            password: value
        }, () => {
            if (value === "") {
                $("#login-password-err").text("Password required");
            } else {
                $("#login-password-err").text("");
            }
        });
    }

    onConfirmPasswordChange = (e) => {
        let value = e.target.value;
        this.setState({
            confirmPassword: value
        }, () => {
            if (value === "") {
                $("#login-confirmPassword-err").text("Confirm password required");
            } else {
                $("#login-confirmPassword-err").text("");
            }
        });
    }

    onFirstNameChange = (e) => {
        let value = e.target.value;
        this.setState({
            firstName: value
        }, () => {
            if (value === "") {
                $("#login-firstName-err").text("First name required");
            } else {
                $("#login-firstName-err").text("");
            }
        });
    }

    onLastNameChange = (e) => {
        let value = e.target.value;
        this.setState({
            lastName: value
        }, () => {
            if (value === "") {
                $("#login-lastName-err").text("Last name  required");
            } else {
                $("#login-lastName-err").text("");
            }
        });
    }

    validateEmail = (value, errorId, displayFieldName) => {
        if (value === "") {
            $("#" + errorId).text("Email address required");
            return false;
        }
        var isValid = /^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$/.test(value);
        if (!isValid)
            $("#" + errorId).text("Invalid " + displayFieldName + " address");
        return isValid;
    }

    validatePassword = (value, errorId, displayFieldName) => {
        if (value === "") {
            $("#" + errorId).text(displayFieldName +" required");
            return false;
        }
        var isValid = value !== "";
        if (!isValid)
            $("#" + errorId).text("Invalid " + displayFieldName);

        if (errorId.includes("confirmPassword") && isValid) {
            if (this.state.password !== value) {
                $("#" + errorId).text("Password not match");
                isValid = false;
            }
        }
        return isValid;
    }

    validateName = (value, errorId, displayFieldName) => {
        if (value === "") {
            $("#" + errorId).text(displayFieldName + " required");
            return false;
        }
        var isValid = /^[a-zA-Z]+$/.test(value);
        if (!isValid)
            $("#" + errorId).text("Invalid " + displayFieldName);
        return isValid;
    }

    onLoginClick = () => {
        const { email, password } = this.state;
        let isValid = true;
        if (!this.validateEmail(email, "login-email-err", "email")) {
            isValid = false;
        }
        if (!this.validatePassword(password, "login-password-err", "password")) {
            isValid = false;
        }
        if (!isValid) {
            return;
        }

        this.setState({
            screenLoading: true
        })
        var data = {
            Email: email,
            Password: password
        };
        CommonPost("api/v1/user", null, data)
            .then(res => {
                console.log('res ',res)
                if (res.isSuccess) {
                    if (res.data != null) {
                        const token = res.data;
                        localStorage.setItem("token", token);
                    }
                    this.setState({
                        screenLoading: false
                    }, () => {
                        this.props.navigate('/dashboard');
                    })

                } else {
                    openNotification(NotificationStatusTypes.Error, "", res.message);
                    this.setState({
                        screenLoading: false
                    })
                }

            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "", "User login failed.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    onRegisterClick = () => {
        const { email, password, confirmPassword, firstName, lastName } = this.state;
        let isValid = true;
        if (!this.validateEmail(email, "login-email-err", "email")) {
            isValid = false;
        }
        if (!this.validatePassword(password, "login-password-err", "password")) {
            isValid = false;
        }
        if (!this.validatePassword(confirmPassword, "login-confirmPassword-err", "confirm password")) {
            isValid = false;
        }
        if (!this.validateName(firstName, "login-firstName-err", "first name")) {
            isValid = false;
        }
        if (!this.validateName(lastName, "login-lastName-err", "last name")) {
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        this.setState({
            screenLoading: true
        })
        var data = {
            Email: email,
            Password: password,
            FirstName: firstName,
            LastName: lastName
        };
        CommonPost("api/v1/user/register", null, data)
            .then(res => {
                if (res.isSuccess) {
                    openNotification(NotificationStatusTypes.Success, "", res.message);
                    this.setState({
                        email: '',
                        password: '',
                        confirmPassword: '',
                        firstName: '',
                        lastName: '',
                        screenLoading: false
                    }, () => {
                        this.accessToLogin();
                    });
                } else {
                    openNotification(NotificationStatusTypes.Error, "", res.message);
                    this.setState({
                        screenLoading: false
                    })
                }

            }).catch(err => {
                openNotification(NotificationStatusTypes.Error, "", "User registration failed.");
                this.setState({
                    screenLoading: false
                })
            })
    }

    accessToRegistration = () => {
        this.setState({
            loginScreenEnabled: false,
            regScreenEnabled: true,
            passwordResetScreenEnabled: false
        })
    }

    accessToLogin = () => {
        this.setState({
            loginScreenEnabled: true,
            regScreenEnabled: false,
            passwordResetScreenEnabled: false
        })
    }

    getLoginDiv = () => {
        return (
            <Card className="login-container">
                <div className="login-screen-logo">
                </div>
                <div >
                    <label className="form-label">Email</label>
                    <Input
                        placeholder="Email"
                        prefix={<UserOutlined className="gray-icon" />}
                        className="w-100"
                        onChange={this.onChangeEmail}
                        value={this.state.email}
                    />
                    <div id="login-email-err" className="error-txt"></div>
                </div>
                <div className="mt-2">
                    <label className="form-label">Password</label>
                    <Input.Password
                        placeholder="Password"
                        prefix={<KeyOutlined className="gray-icon" />}
                        className="w-100"
                        onChange={this.onPasswordChange}
                        value={this.state.password}
                    />
                    <div id="login-password-err" className="error-txt"></div>
                </div>
                <div className="mt-2">
                    <Button className="btn-custom-2 w-100" onClick={this.onLoginClick}>Login</Button>
                </div>
                <Divider className="custom-divider" />
                <div className="mt-3 font-color-white" align="center">
                    <span >Don't have an account? <span className="btn-txt" onClick={this.accessToRegistration}>Register Now</span></span>
                </div>
            </Card>
        );
    }

    getRegDiv = () => {
        return (
            <Card className="reg-container">
                <div >
                    <label className="form-label">First Name</label>
                    <Input
                        placeholder="First Name"
                        className="w-100"
                        onChange={this.onFirstNameChange}
                        value={this.state.firstName}
                    />
                    <div id="login-firstName-err" className="error-txt"></div>
                </div>
                <div >
                    <label className="form-label">Last Name</label>
                    <Input
                        placeholder="Last Name"
                        className="w-100"
                        onChange={this.onLastNameChange}
                        value={this.state.lastName}
                    />
                    <div id="login-lastName-err" className="error-txt"></div>
                </div>
                <div >
                    <label className="form-label">Email</label>
                    <Input
                        placeholder="Email"
                        prefix={<UserOutlined className="gray-icon" />}
                        className="w-100"
                        onChange={this.onChangeEmail}
                        value={this.state.email}
                    />
                    <div id="login-email-err" className="error-txt"></div>
                </div>
                <div className="mt-2">
                    <label className="form-label">Password</label>
                    <Input.Password
                        placeholder="Password"
                        prefix={<KeyOutlined className="gray-icon" />}
                        className="w-100"
                        onChange={this.onPasswordChange}
                        value={this.state.password}
                    />
                    <div id="login-password-err" className="error-txt"></div>
                </div>
                <div className="mt-2">
                    <label className="form-label">Confirm Password</label>
                    <Input.Password
                        placeholder="Confirm Password"
                        prefix={<KeyOutlined className="gray-icon" />}
                        className="w-100"
                        onChange={this.onConfirmPasswordChange}
                        value={this.state.confirmPassword}
                    />
                    <div id="login-confirmPassword-err" className="error-txt"></div>
                </div>
                <div className="mt-2">
                    <Button className="btn-custom-2 w-100" onClick={this.onRegisterClick}>Register</Button>
                </div>
                <Divider className="custom-divider" />
                <div className="mt-3 font-color-white" align="center">
                    <span>Already have an account. <span className="btn-txt" onClick={this.accessToLogin}>Sign In</span></span>
                </div>
            </Card>
        )
    }

    render() {
        return (
            <ScreenLoaderModal loading={this.state.screenLoading}>
                {

                    <div className="login-reg-main-container">
                        {
                            this.state.hideUI ? (
                                <div></div>
                            ) : (

                                <div>

                                    {
                                        this.state.loginScreenEnabled ? (
                                            this.getLoginDiv()
                                        ) : ("")
                                    }

                                    {
                                        this.state.regScreenEnabled ? (
                                            this.getRegDiv()
                                        ) : ("")
                                    }
                                </div>
                            )
                        }


                    </div>
                }


            </ScreenLoaderModal>
        )
    }
}

export default withRouter(LoginRegistration);