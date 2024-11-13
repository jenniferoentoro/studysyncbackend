import * as React from 'react';
import NavbarOutside from "../Components/NavbarOutside.tsx";
import Container from 'react-bootstrap/Container';
import TextField from '@mui/material/TextField';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import AuthenticationAPI from "../APIs/Authentication.tsx";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

function RegisterPage() {
    const MySwal = withReactContent(Swal);
    const [fullName, setFullName] = React.useState('');
    const [email, setEmail] = React.useState('');
    const [password, setPassword] = React.useState('');
    const [grade, setGrade] = React.useState('');
    const [school, setSchool] = React.useState('');
    const [loading, setLoading] = React.useState(false); // State to manage loading status

    const handleGradeChange = (event: SelectChangeEvent) => {
        setGrade(event.target.value);
    };

    const handleSignUp = () => {
        setLoading(true); // Set loading to true when sign up process starts
        AuthenticationAPI.registerUser(email, password, fullName, grade, school)
            .then((data) => {
                // Handle successful registration
                console.log("Registration successful:", data);
                // You may want to redirect to another page or show a success message
                setFullName('');
                setEmail('');
                setPassword('');
                setGrade('');
                setSchool('');
                MySwal.fire({
                    title: "Success!",
                    text: "Registration successful!",
                    icon: "success",
                    confirmButtonText: "OK",
                });
            })
            .catch((response) => {
                console.log("Registration failed:", response);
                const errors = response.response.data.errors;
                let errorMessage = "";
                console.log(errors);

                for (const key in errors) {
                    errorMessage += `${errors[key]}\n`;
                }


                MySwal.fire({
                    title: "Error!",
                    text: errorMessage,
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
                        <h1 className="text-center text-5xl mt-3 font-bold migralight">Register</h1>

                        <div className="frame-background mt-4 mx-auto">
                            <div className="row h-100">
                                <div className="col-12 d-flex flex-column justify-content-center ">

                                    <TextField
                                        id="full-name"
                                        label="Full Name"
                                        variant="standard"
                                        value={fullName}
                                        onChange={(e) => setFullName(e.target.value)}
                                        sx={{width: '100%'}}
                                        className="mt-1"
                                    />

                                    <TextField
                                        id="email"
                                        label="Email"
                                        variant="standard"
                                        value={email}
                                        onChange={(e) => setEmail(e.target.value)}
                                        sx={{width: '100%'}}
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

                                    <FormControl variant="standard" className="mt-2" sx={{width: '100%'}}>
                                        <InputLabel id="grade-select-label">Grade</InputLabel>
                                        <Select
                                            labelId="grade-select-label"
                                            id="grade-select"
                                            value={grade}
                                            label="Grade"
                                            onChange={handleGradeChange}
                                        >
                                            <MenuItem value="">
                                                <em>None</em>
                                            </MenuItem>
                                            <MenuItem value="Elementary">Elementary</MenuItem>
                                            <MenuItem value="Middle School">Middle School</MenuItem>
                                            <MenuItem value="High School">High School</MenuItem>
                                            <MenuItem value="Bachelor">Bachelor</MenuItem>
                                            <MenuItem value="Master">Master</MenuItem>
                                            <MenuItem value="Doctor">Doctor</MenuItem>
                                            <MenuItem value="Professor">Professor</MenuItem>
                                            <MenuItem value="College">College</MenuItem>
                                            <MenuItem value="Unknown">Unknown</MenuItem>
                                        </Select>
                                    </FormControl>

                                    <TextField
                                        id="school"
                                        label="School"
                                        variant="standard"
                                        value={school}
                                        onChange={(e) => setSchool(e.target.value)}
                                        sx={{width: '100%'}}
                                    />

                                    <p>Already have an account? <a href="/login" className="no-underline">Log in</a></p>

                                    <button
                                        className="animate__animated animate__fadeIn rounded-full color-gradient text-white px-20 tracking-wider py-2 mt-3"
                                        onClick={handleSignUp}
                                        disabled={loading} // Disable the button when loading
                                    >
                                        {loading ? 'Loading...' : 'Sign Up'} {/* Show 'Loading...' when loading */}
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

export default RegisterPage;
