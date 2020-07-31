import React, {useEffect, useState} from 'react';
import axios from "axios";
import Header from "./Components/Header";
import {
    Grid,
    TableContainer,
    Table,
    TableHead,
    TableRow,
    TableCell,
    TableBody,
    Paper,
    CircularProgress,
    Container,
    Box, Typography, TablePagination
} from "@material-ui/core";
import {makeStyles} from "@material-ui/core/styles";

const useStyles = makeStyles((theme) => ({
    gridContainer: {
        paddingTop: theme.spacing(2)
    },

    tableContainer: {
        display: "flex",
        justifyContent: "center",
        alignItems: "center"
    }
}));



const Home = () => {
    const classes = useStyles();

    const [airports, setAirports] = useState();
    const [paginationInfo, setPaginationInfo] = useState();

    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);



    useEffect(() => {

        axios.get("https://localhost:6001/api/airports")
            .then(res => {
                console.log(res.headers["x-pagination"]);
                setAirports(res.data);
                setPaginationInfo(JSON.parse(res.headers["x-pagination"]));
            })
            .catch(err => console.error(err));

    }, []);


    const displayAirports = (airports) => {

        if (!(airports !== undefined && airports.length > 1 && paginationInfo !== undefined))
            return <CircularProgress/>;


        const handleChangePage = (event, newPage) => {
            setPage(newPage);
        };

        const handleChangeRowsPerPage = (event) => {
            setRowsPerPage(+event.target.value);
            setPage(0);
        };

        console.log(paginationInfo.ItemCount);

        return (
            <Paper>
                <TableContainer>
                    <Table stickyHeader aria-label="sticky table">
                        <TableHead>
                            <TableRow>
                                <TableCell>Id</TableCell>
                                <TableCell align="left">Name</TableCell>
                                <TableCell align="left">Country</TableCell>
                                <TableCell align="left">City</TableCell>
                                <TableCell align="left">IATA</TableCell>
                                <TableCell align="left">ICIAO</TableCell>
                                <TableCell align="left">Timezone</TableCell>
                                <TableCell align="left">Latitude</TableCell>
                                <TableCell align="left">Longitude</TableCell>
                                <TableCell align="left">Altitude</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {airports.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((airport) => (
                                <TableRow key={airport.id}>
                                    <TableCell component="th" scope="row">
                                        {airport.id}
                                    </TableCell>
                                    <TableCell align="left">{airport.name}</TableCell>
                                    <TableCell align="left">{airport.country}</TableCell>
                                    <TableCell align="left">{airport.city}</TableCell>
                                    <TableCell align="left">{airport.iata}</TableCell>
                                    <TableCell align="left">{airport.iciao}</TableCell>
                                    <TableCell align="left">{airport.timezone}</TableCell>
                                    <TableCell align="left">{airport.latitude}</TableCell>
                                    <TableCell align="left">{airport.longitude}</TableCell>
                                    <TableCell align="left">{airport.altitude}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
                <TablePagination
                    rowsPerPageOptions={[10, 25, 50]}
                    component="div"
                    count={airports.length}
                    rowsPerPage={rowsPerPage}
                    page={page}
                    onChangePage={handleChangePage}
                    onChangeRowsPerPage={handleChangeRowsPerPage}
                />
            </Paper>
        );
    };


    return (
        <>
            <Header/>
            <Grid
                container
                direction="row"
                justify="center"
                className={classes.gridContainer}
            >
                <Grid item xs={12}>
                    <Container maxWidth="lg" className={classes.tableContainer}>
                        {displayAirports(airports)}
                    </Container>
                </Grid>
            </Grid>
            <Box component="footer" mt={2} p={2} bgcolor="primary.main" color="white">
                <Typography>
                    Eagle Airlines &copy;2020
                </Typography>
            </Box>
        </>
    );
};

export default Home;