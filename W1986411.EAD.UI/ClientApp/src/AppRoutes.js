import CheatMealPlan from "./components/CheatMealPlan";
import { Home } from "./components/Home";
import { Layout } from "./components/Layout";
import LoginRegistration from "./components/LoginRegistration";
import Unauthorize from "./components/Utils/Unauthorize";
import WorkoutPlans from "./components/WorkoutPlans";
import Dashboard from "./components/Dashboard";

const AppRoutes = [
    {
        index: true,
        element: <LoginRegistration />
    },
    {
        path: '/dashboard',
        element: <Unauthorize><Layout><Dashboard /></Layout></Unauthorize>
    },
    {
        path: '/workout-plans',
        element: <Unauthorize><Layout><WorkoutPlans /></Layout></Unauthorize>
    },
    {
        path: '/cheatmeal-plans',
        element: <Unauthorize><Layout><CheatMealPlan /></Layout></Unauthorize>
    }
];

export default AppRoutes;
