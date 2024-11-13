import * as React from 'react';
import NavbarOutside from "../Components/NavbarOutside.tsx";
import Container from 'react-bootstrap/Container';
import TextField from '@mui/material/TextField';
import AuthenticationAPI from "../APIs/Authentication.tsx";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { useNavigate } from "react-router-dom";

function LoginPage() {
    const MySwal = withReactContent(Swal);
    const [email, setEmail] = React.useState('');
    const [password, setPassword] = React.useState('');
    const [loading, setLoading] = React.useState(false); // State to manage loading status
    const navigate = useNavigate();

    const handleLogin = () => {
        setLoading(true); // Set loading to true when sign up process starts
        AuthenticationAPI.loginUser(email, password)
            .then((data) => {
                console.log("Login successful:", data);
                setEmail('');
                setPassword('');

                const role = data.role;
                if (role === "Admin") {
                    navigate("/admin");
                } else if (role === "User") {
                    navigate("/home");
                }
            })
            .catch((error) => {
                console.error("Registration failed:", error);
                MySwal.fire({
                    title: "Error!",
                    text: error,
                    icon: "error",
                    confirmButtonText: "OK",
                });
            })
            .finally(() => {
                setLoading(false); // Set loading to false when sign up process completes
            });
    };

    return (
        <div className="cover-background min-vh-100">
            <NavbarOutside />
            <Container>
                <div className="row justify-content-center">
                    <div className="col-6">
                        <h1 className="text-center text-5xl mt-3 font-bold migralight">Welcome back!</h1>

                        <div className="frame-background mt-4 mx-auto">
                            <div className="row h-100">
                                <div className="col-12 d-flex flex-column justify-content-center ">

                                    <TextField
                                        id="email"
                                        label="Email"
                                        variant="standard"
                                        value={email}
                                        onChange={(e) => setEmail(e.target.value)}
                                        sx={{width: '100%'}}
                                        style={{marginTop: '50px' }}
                                    />
                                    <TextField
                                        id="password"
                                        label="Password"
                                        type="password"
                                        variant="standard"
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        sx={{width: '100%'}}
                                    />


                                    <p>Do not have an account? <a href="/register" className="no-underline">Register</a></p>

                                    <button
                                        className="animate__animated animate__fadeIn rounded-full color-gradient text-white px-20 tracking-wider py-2 mt-3"
                                        onClick={handleLogin}
                                        disabled={loading} // Disable the button when loading
                                    >
                                        {loading ? 'Loading...' : 'Login'} {/* Show 'Loading...' when loading */}
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </Container>
        </div>
    );
}

export default LoginPage;
