import React from 'react';
import ReactDOM from 'react-dom/client';
import './styles/index.css';
// import App from './App';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { UserApp } from './pages/User/UserApp.tsx';
import { LoginPage } from './pages/Auth/LoginPage.tsx';
import { UserHomePage } from './pages/User/UserHomePage.tsx';


const router = createBrowserRouter([
  {
    path: "/",
    element: <UserApp />,
    children: [
      {
        path: "/",
        element: <UserHomePage />
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
