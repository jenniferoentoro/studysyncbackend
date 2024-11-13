import Container from 'react-bootstrap/Container';
import NavbarOutside from "../Components/NavbarOutside.tsx";

import { Link } from "react-router-dom";
import "animate.css";
function LandingPage(){
    return (
        <div className="cover-background min-vh-100">
        <NavbarOutside />
        <Container fluid>
            <div className="row mt-48">
                <div className="col-12 col-md-4 col-lg-4 col-xl-4">
                    <div className="mr-10 md:mr-0 ml-10 md:ml-20 sm:mt-0 lg:mt-16 text-center migralight">
                        <h1 className="animate__animated animate__backInDown text-5xl lg:text-8xl font-bold">
                            Study
                        </h1>
                        <h1 className="ml-24 lg:ml-48 animate__animated animate__backInDown text-5xl lg:text-8xl font-bold ">
                            Sync
                        </h1>
                        <p className="animate__animated animate__lightSpeedInLeft text-xl mt-4 text-grey italic">
                            Find your study group and sync your study time with them.
                        </p>
                        <Link to="/login">
                            <button
                                className="animate__animated animate__fadeIn rounded-full bg-[#ebcfe2] text-black px-20 tracking-wider py-2 mt-2">
                                Sign In
                            </button>
                        </Link>
                        <br/>
                        <Link to="/register">
                            <button
                                className="animate__animated animate__fadeIn rounded-full bg-[#cfa5c6] text-white px-20 tracking-wider py-2 mt-2">
                                Sign Up
                            </button>
                        </Link>
                    </div>
                </div>
                <div className="col-12 col-md-8 col-lg-8 col-xl-8">
                    <div className="text-center">
                    <img
                            src="/images/home-page.png"
                            className="animate__animated animate__zoomIn d-inline-block align-top w-auto"
                            alt="Home Logo"
                        />
                    </div>
                </div>
            </div>
        </Container>
        </div>
    )
}
export default LandingPage;