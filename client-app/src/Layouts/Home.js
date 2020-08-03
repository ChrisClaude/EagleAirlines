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
    TableSortLabel,
    TablePagination,
    Paper,
    CircularProgress,
    Container,
    Box, Typography,
} from "@material-ui/core";
import {makeStyles} from "@material-ui/core/styles";
import PropTypes from "prop-types";
import AutoComplete from "./Components/AutoComplete";

const useStyles = makeStyles((theme) => ({
    gridContainer: {
        paddingTop: theme.spacing(2)
    },

    loaderContainer: {
        display: "flex",
        justifyContent: "center",
        alignItems: "center"
    },

    tableRoot: {
        width: "100%"
    },

    visuallyHidden: {
        border: 0,
        clip: 'rect(0 0 0 0)',
        height: 1,
        margin: -1,
        overflow: 'hidden',
        padding: 0,
        position: 'absolute',
        top: 20,
        width: 1,
    },

    searchGrid: {
        marginBottom: theme.spacing(2)
    },
}));

function EnhancedTableHead(props) {
    const {classes, order, orderBy, onRequestSort} = props;
    const createSortHandler = (property) => (event) => {
        onRequestSort(event, property);
    };

    const headCells = [
        // {id: 'Id', numeric: true}, // TODO: sorting by id needs to be added on the server
        {id: 'Name', numeric: false},
        {id: 'Country', numeric: false},
        {id: 'City', numeric: false},
    ];


    return (
        <TableHead>
            <TableRow>
                <TableCell align="left">Id</TableCell>
                {headCells.map((headCell) => (
                    <TableCell
                        key={headCell.id}
                        sortDirection={orderBy === headCell.id ? order : false}
                    >
                        <TableSortLabel
                            active={orderBy === headCell.id}
                            direction={orderBy === headCell.id ? order : 'asc'}
                            onClick={createSortHandler(headCell.id)}
                        >
                            {headCell.id}
                            {orderBy === headCell.id ? (
                                <span className={classes.visuallyHidden}>
                  {order === 'desc' ? 'sorted descending' : 'sorted ascending'}
                </span>
                            ) : null}
                        </TableSortLabel>
                    </TableCell>
                ))}
                <TableCell align="left">IATA</TableCell>
                <TableCell align="left">ICIAO</TableCell>
                <TableCell align="left">Timezone</TableCell>
                <TableCell align="left">Latitude</TableCell>
                <TableCell align="left">Longitude</TableCell>
                <TableCell align="left">Altitude</TableCell>
            </TableRow>
        </TableHead>
    );
}

const Home = () => {
    const classes = useStyles();

    const [airports, setAirports] = useState();
    const [paginationInfo, setPaginationInfo] = useState();

    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);

    const [order, setOrder] = React.useState('asc');
    const [orderBy, setOrderBy] = React.useState('name');


    useEffect(() => {

        axios.get(`https://localhost:6001/api/airports?pageSize=${rowsPerPage}`)
            .then(res => {
                setAirports(res.data);
                setPaginationInfo(JSON.parse(res.headers["x-pagination"]));
            })
            .catch(err => console.error(err));

    }, [rowsPerPage]);


    const handleRequestSort = (event, property) => {
        const isAsc = orderBy === property && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        setOrderBy(property);

        axios.get(`https://localhost:6001/api/airports?pageIndex=${page + 1}` +
            `&pageSize=${rowsPerPage}&sort=${isAsc ? property.toLowerCase() + "_desc" : property.toLowerCase()}`)
            .then(res => {
                setAirports(res.data);
                setPaginationInfo(JSON.parse(res.headers["x-pagination"]));
            })
            .catch(err => console.error(err));

        setPage(0);

        console.log(`orderBy ${orderBy}, order ${order}`);
    };


    const displayAirports = (airports) => {

        if (!(airports !== undefined && airports.length > 1 && paginationInfo !== undefined))
            return (
                <Container className={classes.loaderContainer}>
                    <CircularProgress/>
                </Container>
            );


        const handleChangePage = (event, newPage) => {

            if (newPage > paginationInfo.TotalPages) {
                newPage = 0;
            }


            axios.get(`https://localhost:6001/api/airports?pageIndex=${newPage + 1}&pageSize=${rowsPerPage}`)
                .then(res => {
                    setAirports(res.data);
                    setPaginationInfo(JSON.parse(res.headers["x-pagination"]));
                })
                .catch(err => console.error(err));

            setPage(newPage);
        };

        const handleChangeRowsPerPage = (event) => {
            setRowsPerPage(event.target.value);

            axios.get(`https://localhost:6001/api/airports?pageSize=${event.target.value}`)
                .then(res => {
                    setAirports(res.data);
                    setPaginationInfo(JSON.parse(res.headers["x-pagination"]));
                })
                .catch(err => console.error(err));

            setPage(0);
        };

        return (
            <Paper className={classes.tableRoot}>
                <TableContainer>
                    <Table stickyHeader aria-label="sticky table">
                        <EnhancedTableHead
                            classes={classes}
                            order={order}
                            orderBy={orderBy}
                            onRequestSort={handleRequestSort}
                        />
                        <TableBody>
                            {airports.map((airport) => (
                                <TableRow key={airport.id} hover>
                                    <TableCell component="th" scope="row">
                                        {airport.id}
                                    </TableCell>
                                    <TableCell align="left">{airport.name}</TableCell>
                                    <TableCell align="left">{airport.country}</TableCell>
                                    <TableCell align="left">{airport.city}</TableCell>
                                    <TableCell align="left">{airport.iata}</TableCell>
                                    <TableCell align="left">{airport.iciao}</TableCell>
                                    <TableCell
                                        align="left">{airport.timezone > 0 ? "+" + airport.timezone : airport.timezone}</TableCell>
                                    <TableCell
                                        align="left">{airport.latitude > 0 ? "+" + airport.latitude : airport.latitude}</TableCell>
                                    <TableCell
                                        align="left">{airport.longitude > 0 ? "+" + airport.longitude : airport.longitude}</TableCell>
                                    <TableCell
                                        align="left">{airport.altitude > 0 ? "+" + airport.altitude : airport.altitude}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
                <TablePagination
                    rowsPerPageOptions={[10, 25, 50]}
                    component="div"
                    count={-1}
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
            <Container>
                <Grid
                    container
                    direction="row"
                    justify="center"
                    className={classes.gridContainer}
                >
                    <Grid item xs={12} sm={6} className={classes.searchGrid}>
                        <AutoComplete srcUrl="https://localhost:6001/api/airports" label="Search airports"/>
                    </Grid>
                    <Grid item xs={12}>
                        {displayAirports(airports)}
                    </Grid>
                </Grid>

            </Container>
            <Box component="footer" mt={2} p={2} bgcolor="primary.main" color="white">
                <Typography>
                    Eagle Airlines &copy;2020
                </Typography>
            </Box>
        </>
    );
};

export default Home;


EnhancedTableHead.propTypes = {
    classes: PropTypes.object.isRequired,
    onRequestSort: PropTypes.func.isRequired,
    order: PropTypes.oneOf(['asc', 'desc']).isRequired,
    orderBy: PropTypes.string.isRequired,
};