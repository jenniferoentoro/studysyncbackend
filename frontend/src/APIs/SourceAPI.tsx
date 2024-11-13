import axios from "axios";



const SourceAPI = {
    // getSources: () => {
    //     const accessToken = localStorage.getItem("accessToken");
    //     const headers = {
    //         Authorization: `Bearer ${accessToken}`,
    //     };
    //     return axios
    //         .get("http://localhost:5280/api/source", { headers })
    //         .then((response) => response.data);
    //
    // },
    getSources : (title?: string) => {
        const accessToken = localStorage.getItem("accessToken");
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        };

        let url = "http://localhost:5280/api/source";
        if (title) {
            url += `?Title=${encodeURIComponent(title)}`;
        }

        return axios
            .get(url, { headers })
            .then((response) => response.data);
    },

    getSourceById: (id: number) => {
        const accessToken = localStorage.getItem("accessToken");
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        };
        return axios
            .get(`http://localhost:5280/api/source/${id}`, { headers })
            .then((response) => response.data);
    },
    addSource: (formData: { title: string; description: string; categoryId: number; userId: number; file: File| null; }) => {
        const accessToken = localStorage.getItem("accessToken");
        const headers = {
            Authorization: `Bearer ${accessToken}`,
            "Content-Type": "multipart/form-data",
        };

        const data = new FormData();
        data.append("Title", formData.title);
        data.append("Description", formData.description);
        data.append("CategoryId", String(formData.categoryId));
        data.append("UserId", String(formData.userId));
        if (formData.file) {
            data.append("File", formData.file);
        }

        // Make the POST request
        return axios
            .post("http://localhost:5280/api/source", data, { headers })
            .then((response) => response.data);
    },
};

export default SourceAPI;
