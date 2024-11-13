import axios from "axios";



const AuthenticationAPI = {
    registerUser: (
        email: string,
        password: string,
        name: string,
        grade: string,
        school: string,
    ) => {
        return axios
            .post("http://localhost:5225/api/user/user", {
                email,
                password,
                name,
                grade,
                school,
            })
            .then((response) => response.data);
    },


    loginUser: (email:string, password:string) => {
        return axios
            .post("http://localhost:5225/api/user/login", {
                email,
                password,
            })
            .then((response) => {
                const { token } = response.data; // Extract token from response data
                localStorage.setItem("accessToken", token.toString()); // Store token in localStorage
                return response.data; // Return response data
            });
    },
};

export default AuthenticationAPI;
