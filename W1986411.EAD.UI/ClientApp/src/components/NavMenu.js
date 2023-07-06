import React, { Component } from 'react';
import {
    MenuOutlined,
    ArrowLeftOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined,
    LineChartOutlined,
    PieChartOutlined
} from '@ant-design/icons';
import { Layout, Menu, Drawer, Button, Space, Popover, Avatar, Divider } from 'antd';
import { withRouter } from '../withRouter';
import { IoPersonOutline, IoBody } from 'react-icons/io5';
import jwt from 'jwt-decode';
import { CommonGet } from './Utils/CommonFetch';

const { Header, Sider } = Layout;

class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: false,
            selectedMenu: '1',
            drawerOpen: false,
            email: '',
            userFullName: ''
        };
    }

    componentDidMount() {
        if (window.location.pathname === "/workout-plans") {
            this.setState({
                selectedMenu: '2'
            })
        }

        var token = localStorage.getItem("token");

        if (token !== null && token !== "null" && token !== undefined) {
            const user = jwt(token);
            const email = user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
            const userFullName = user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

            this.setState({
                userFullName,
                email
            })
        }
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    navigateMenu = (value) => {
        if (value.key === "1") {
            this.props.navigate('/historical');
        } else {
            this.props.navigate('/workout-plans');
        }
        this.setState({
            selectedMenu: value.key
        })
    }

    toggleDrawer = () => {
        this.setState({
            drawerOpen: !this.state.drawerOpen
        });
    }

    logoutOnClick = () => {
        localStorage.removeItem("token");
        this.props.navigate("/");
    }

    render() {
        var userIconContent =
            <div className="row nav-user-dropdown">
                <div className="col-md-12" align="right">
                    <span className="nav-user-dropdown-user-name">{this.state.userFullName}</span>
                </div>
                <div className="col-md-12" align="right">
                    <span className="nav-user-dropdown-user-email">{this.state.email}</span>
                </div>
                <div className="col-md-12" align="right">
                    <Button className="btn-custom-2 mt-2 mb-2 btn-size-1" onClick={this.logoutOnClick}>Logout</Button>
                </div>

            </div>;
        return (
            <Layout >
                <Sider trigger={null} collapsible collapsed={this.state.collapsed} className="header-side-menu" >
                    <div>
                        <div align="center" className="side-menu-collapse-icon">
                            <MenuOutlined onClick={this.toggleNavbar} />
                        </div>
                    </div>
                    <Menu
                        className="header-side-menu-items"
                        mode="inline"
                        onClick={this.navigateMenu}
                        items={[
                            {
                                key: '1',
                                icon: <PieChartOutlined />,
                                label: 'Dashboard',
                                title: ''
                            },
                            {
                                key: '2',
                                icon: < IoBody />,
                                label: 'Workout Plans',
                                title: '',
                            }
                        ]}
                        selectedKeys={[this.state.selectedMenu]}
                    />
                </Sider>

                <Layout>
                    <Header className="site-header-nav">
                        <div>
                            <div className="header-drawer">
                                <div
                                    className=" d-flex flex-row ">
                                    <div align="left" className="side-menu-collapse-icon-mobile flex-grow-1">
                                        <MenuOutlined onClick={this.toggleDrawer} />
                                    </div>
                                    <div className="flex-grow-1">
                                        <div
                                            className="nav-user-container" align="right" >
                                            <Popover overlayClassName="nav-user-popup-container" placement="bottom" content={userIconContent} >
                                                <Avatar size={30} className="white-bg-color" icon={<IoPersonOutline />} />
                                            </Popover>
                                        </div>
                                    </div>
                                </div>

                                {/*<Drawer*/}
                                {/*    closeIcon={<ArrowLeftOutlined className="nav-drawer-close-icon" />}*/}
                                {/*    placement="left"*/}
                                {/*    onClose={this.toggleDrawer}*/}
                                {/*    open={this.state.drawerOpen}*/}
                                {/*    className="nav-drawer"*/}
                                {/*>*/}
                                {/*    <Menu*/}
                                {/*        className="header-side-menu-items-drawer"*/}
                                {/*        mode="inline"*/}
                                {/*        onClick={this.navigateMenu}*/}
                                {/*        items={[*/}
                                {/*            {*/}
                                {/*                key: '1',*/}
                                {/*                icon: <PieChartOutlined />,*/}
                                {/*                label: 'Dashboard',*/}
                                {/*                title: ''*/}
                                {/*            },*/}
                                {/*            {*/}
                                {/*                key: '2',*/}
                                {/*                icon: < LineChartOutlined />,*/}
                                {/*                label: 'Workout Plans',*/}
                                {/*                title: '',*/}
                                {/*            }*/}
                                {/*        ]}*/}
                                {/*        selectedKeys={[this.state.selectedMenu]}*/}
                                {/*    />*/}
                                {/*</Drawer>*/}
                            </div>

                            <div className="nav-header">
                                <div
                                    className="nav-user-container" align="right" >
                                    <Popover overlayClassName="nav-user-popup-container" placement="bottom" content={userIconContent} >
                                        <Avatar size={30} className="white-bg-color" icon={<IoPersonOutline />} />
                                    </Popover>
                                </div>
                            </div>

                        </div>

                    </Header>
                    <div className="main-container">
                        {this.props.children}
                    </div>
                </Layout>
            </Layout>
        );
    }
}

export default withRouter(NavMenu);