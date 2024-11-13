import React, { useEffect, useState } from "react";
import NavbarInside from "../../Components/NavbarInside";
import Container from 'react-bootstrap/Container';
import SearchIcon from "@mui/icons-material/Search";
import FilterAltIcon from "@mui/icons-material/FilterAlt";
import './HomePage.css';
import Popover from "@mui/material/Popover";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import Checkbox from "@mui/material/Checkbox";
import CategoryAPI from "../../APIs/CategoryAPI";
import SourceAPI from "../../APIs/SourceAPI.tsx";
import Button from "@mui/material/Button";
import {Source} from "@mui/icons-material";
import Card from "react-bootstrap/Card";
import { Link } from "react-router-dom";
import PersonIcon from '@mui/icons-material/Person';
interface Category {
    id: number;
    name: string;
}

interface Source {
    id: number;
    title: string;
    description: string;
    createdOn: string;
    urlFile: string;
    fileName: string;
    category: {
        id: number;
        name: string;
    };
    user: {
        id: number;
        email: string;
    };
}

const HomePage: React.FC = () => {
    const [anchorEl, setAnchorEl] = useState<HTMLButtonElement | null>(null);
    const [searchQuery, setSearchQuery] = useState<string>("");
    const [selectedCategories, setSelectedCategories] = useState<string[]>([]);
    const isCategorySelected = (categoryName: string) =>
        selectedCategories.includes(categoryName);


    const [sources, setSources] = useState<Source[]>([]);

    useEffect(() => {
        async function fetchSources() {
            try {
                const sourceResponse = await SourceAPI.getSources();
                setSources(sourceResponse);
                console.log(sourceResponse);

                console.log(sourceResponse.Sources);
            } catch (error) {
                console.error("Error fetching news:", error);
            }
        }
        fetchSources();

    }, []);

    const open = Boolean(anchorEl);
    const id = open ? "category-popover" : undefined;
    const handleSearchChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const newSearchQuery = event.target.value;
        setSearchQuery(newSearchQuery);
        // Call the API to get news based on the search query

        if (newSearchQuery === "") {
            try {
                const sourceResponse = await SourceAPI.getSources();
                setSources(sourceResponse);
            } catch (error) {
                console.error("Error fetching source:", error);
            }
        } else {
            try {
              const sourceResponse = await SourceAPI.getSources(newSearchQuery)
              setSources(sourceResponse);
            } catch (error) {
              console.error("Error fetching news:", error);
            }
        }
    };



    const handleFilterIconClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const [categoryList, setCategoryList] = useState<Category[]>([]);

    useEffect(() => {
        async function fetchCategories() {
            try {
                const categories = await CategoryAPI.getCategory();
                setCategoryList(categories);
            } catch (error) {
                console.error("Error fetching incident categories:", error);
            }
        }
        fetchCategories();
    }, []);

    useEffect(() => {
        console.log(selectedCategories);
    }, [selectedCategories]);

    const handleCategoryCheck = (categoryName: string) => {
        setSelectedCategories((prevSelectedCategories) => {
            if (!prevSelectedCategories.includes(categoryName)) {
                return [...prevSelectedCategories, categoryName];
            } else {
                return prevSelectedCategories.filter((name) => name !== categoryName);
            }
        });

    };

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleString();
    };
    // const handleCategoryCheck = (categoryName: string) => {
    //     console.log(categoryName);
    //
    //     setSelectedCategories((prevSelectedCategories) => {
    //         console.log(prevSelectedCategories);
    //
    //         if (!prevSelectedCategories.includes(categoryName)) {
    //
    //             return prevSelectedCategories.filter((name) => name !== categoryName);
    //         } else {
    //             return [...prevSelectedCategories, categoryName];
    //         }
    //     });
    //     console.log(selectedCategories.length);
    //
    //     console.log(selectedCategories);
    // };

    return (
        <div className="cover-background min-vh-100">
            <NavbarInside />
            <Container>
                <h1 className="text-center">Explore</h1>
                <div className="row height d-flex justify-content-center align-items-center mt-3">
                    <div className="col-md-6">
                        <div className="form">
                            <SearchIcon className="searcha"/>
                            <input
                                type="text"
                                className="form-control form-input"
                                placeholder="Search anything..."
                                value={searchQuery}
                                onChange={handleSearchChange}
                            />
                            <span className="left-pan">

                            <Button onClick={handleFilterIconClick}
                                    style={{background: "transparent", border: "none", padding: 0, color: "black"}}>
                                    <FilterAltIcon className="micr cursor-pointer"/>
                                </Button>

                    <Popover
                        id={id}
                        open={open}
                        anchorEl={anchorEl}
                        onClose={() => setAnchorEl(null)}
                    >
                      <List>
                        {categoryList.map((category) => (
                            <ListItem
                                key={category.id}
                                button
                                onClick={() => handleCategoryCheck(category.name)} // Use category.name
                            >
                                <Checkbox
                                    checked={isCategorySelected(category.name)}
                                />{" "}
                                {/* Use category.name */}
                                <ListItemText primary={category.name}/>
                            </ListItem>
                        ))}
                      </List>

                    </Popover>
                  </span>
                        </div>
                    </div>
                </div>

                <div className="row justify-content-center text-center mt-3">
                    {sources.map((sourceItem) => (

                        <div
                            className="col-12 col-md-5 col-lg-4 col-xl-3 my-2"
                            key={sourceItem.id}
                        >

                            <Link to={`/source/${sourceItem.id}`} className="no-underline">
                                <Card
                                    className="cursor-pointer bg-transparent "
                                    style={{height: "100%"}}
                                >


                                    <Card.Body  style={{height: "100%"}}>
                                        <div className="d-flex  align-items-center mb-2">
                                            <div className="mr-2">
                                                <PersonIcon className="text-[#474646]"/>

                                            </div>
                                            <div>
                                                <a className="font-bold no-underline text-[#474646]"
                                                   style={{textDecoration: 'none !important'}}> {sourceItem.user.email}</a>

                                            </div>


                                        </div>
                                        <Card
                                            className="cursor-pointer bg-transparent text-[#474646] p-2 px-3"
                                            // style={{height: "100%"}}
                                        >
                                            <Card.Title className="font-bold text-left text-[#474646]">
                                                {sourceItem.title}
                                            </Card.Title>
                                            <Card.Text className="text-left text-sm">
                                                {sourceItem.description.split(' ').slice(0, 10).join(' ')}
                                                {sourceItem.description.split(' ').length > 10 && '...'}

                                            </Card.Text>


                                        </Card>

                                        <div className="d-flex justify-content-between align-items-end mt-2">
                                                <span
                                                    className="badge badge-primary  px-1 rounded-full bg-[#cfa5c6]">
                                                    {sourceItem.category.name}
                                                </span>
                                            <div className="float-right text-sm">
                                                {formatDate(sourceItem.createdOn)}
                                            </div>
                                        </div>

                                    </Card.Body>
                                </Card>
                            </Link>
                        </div>
                    ))}
                </div>

                <div className="row">
                    <div className="col-12 text-center my-2">
                        <button className="btn" style={{backgroundColor: '#6e646a', color: 'white'}}>Load more</button>
                    </div>
                </div>

            </Container>
        </div>
    );
}

export default HomePage;
