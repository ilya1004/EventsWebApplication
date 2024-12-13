import React from 'react';
import ReactDOM from 'react-dom/client';
import './styles/index.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { UserApp, userAppLoader } from './pages/User/UserApp.tsx';
import { LoginPage } from './pages/Auth/LoginPage.tsx';
import { UserHomePage } from './pages/User/UserHomePage.tsx';
import { userProfileLoader, UserProfilePage } from './pages/User/UserProfile/UserProfile.tsx';
import { userEventsLoader, UserEventsPage } from './pages/User/UserEventsPage.tsx';


const router = createBrowserRouter([
  {
    path: "/",
    element: <UserApp />,
    loader: userAppLoader,
    children: [
      {
        path: "/",
        element: <UserHomePage />,
      },
      {
        path: "my-events",
        element: <UserEventsPage />,
        loader: userEventsLoader,
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
