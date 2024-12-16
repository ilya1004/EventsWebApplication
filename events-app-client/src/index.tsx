import React from 'react';
import ReactDOM from 'react-dom/client';
import './styles/index.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { UserApp, userAppLoader } from './pages/User/UserApp.tsx';
import { LoginPage } from './pages/Auth/LoginPage.tsx';
import { userHomeLoader, UserHomePage } from './pages/User/UserHomePage.tsx';
import { userProfileLoader, UserProfilePage } from './pages/User/UserProfile/UserProfile.tsx';
import { userEventsLoader, UserEventsPage } from './pages/User/UserEventsPage.tsx';
import { userEventInfoLoader, UserEventInfoPage } from './pages/User/UserEventInfo.tsx';
import { RegisterPage } from './pages/Auth/RegisterPage.tsx';
import { adminHomeLoader, AdminHomePage } from './pages/Admin/AdminHomePage.tsx';
import { AdminApp, adminAppLoader } from './pages/Admin/AdminApp.tsx';
import { adminEventInfoLoader, AdminEventInfoPage } from './pages/Admin/AdminEventInfo.tsx';
import { adminEditEventLoader, AdminEditEventPage } from './pages/Admin/AdminEditEventPage.tsx';
import { AdminCreateEventPage } from './pages/Admin/AdminCreateEventPage.tsx';


const router = createBrowserRouter([
  {
    path: "/",
    element: <UserApp />,
    loader: userAppLoader,
    children: [
      {
        path: "/",
        element: <UserHomePage />,
        loader: userHomeLoader,
      },
      {
        path: "my-events",
        element: <UserEventsPage />,
        loader: userEventsLoader,
      },
      {
        path: "event/:eventId",
        element: <UserEventInfoPage />,
        loader: userEventInfoLoader
      },
      {
        path: "my-profile",
        element: <UserProfilePage />,
        loader: userProfileLoader,
        // children: [
        //   {
        //     path: "edit",
        //     element: 
        //   }
        // ]
      }
    ]
  },
  {
    path: "/login",
    element: <LoginPage />
  },
  {
    path: "/register",
    element: <RegisterPage />
  },
  {
    path: "/",
    element: <AdminApp />,
    loader: adminAppLoader,
    children: [
      {
        path: "admin",
        element: <AdminHomePage />,
        loader: adminHomeLoader,
      },
      {
        path: "admin/create-event",
        element: <AdminCreateEventPage />
      },
      {
        path: "admin/event/:eventId",
        element: <AdminEventInfoPage />,
        loader: adminEventInfoLoader,
      },
      {
        path: "admin/edit-event/:eventId",
        element: <AdminEditEventPage />,
        loader: adminEditEventLoader,
      },
    ]
  }
]);


const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
