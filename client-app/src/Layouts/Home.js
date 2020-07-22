import React from 'react';
import Box from "@material-ui/core/Box";
import Container from "@material-ui/core/Container";
import Button from "@material-ui/core/Button";
import axios from "axios";


const getAirports = () => {
    axios.get("https://localhost:6001/api/airports")
        .then(res => console.log(res))
        .catch(err => console.error(err));
};

const Home = () => {

    getAirports();

    return (
        <div>
            <Box className="bg-primary text-white cus-header">
                <Container className="py-3 w-100">
                    <Box className="w-50 mr-5">
                        Eagle Airlines
                    </Box>
                    <Box>
                        <Button href="#contained-buttons">
                            All Destinations
                        </Button>
                    </Box>
                </Container>
            </Box>

            <div className="container">
                Airports
            </div>
        </div>
    );
};

export default Home;