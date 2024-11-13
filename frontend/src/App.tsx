// import reactLogo from './assets/react.svg'
// import viteLogo from '/vite.svg'
import './App.css'
import { Route, Routes, BrowserRouter as Router } from "react-router-dom";
import LandingPage from "./Pages/LandingPage.tsx";
import RegisterPage from "./Pages/RegisterPage.tsx";
import LoginPage from "./Pages/Login.tsx";
import HomePage from "./Pages/User/HomePage.tsx";
import SourceDetail from "./Pages/User/SourceDetail.tsx";
import MySourcePage from "./Pages/User/MySourcePage.tsx";
function App() {

  return (
    <>
        <Router>
            <Routes>
            <Route path="/" element={<LandingPage />} />
            <Route path="/register" element={<RegisterPage />} />
                <Route path="/login" element={<LoginPage />} />

                <Route path="*" element={<h1>Not Found</h1>} />

                <Route path="/home" element={<HomePage/>} />
                <Route path="/source/:id" element={<SourceDetail />} />

                <Route path="/mysources" element={<MySourcePage />} />


            </Routes>
        </Router>
    </>
  )
}

export default App
