import axios from "axios";



const CategoryAPI = {
   getCategory: () => {
       const accessToken = localStorage.getItem("accessToken");
       const headers = {
           Authorization: `Bearer ${accessToken}`,
       };
        return axios
            .get("http://localhost:5280/api/category", { headers })
            .then((response) => response.data);

   },
};

export default CategoryAPI;
