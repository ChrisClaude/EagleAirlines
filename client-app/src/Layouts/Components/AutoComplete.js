import axios from "axios";
import React, {Fragment, useEffect, useState} from 'react';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import CircularProgress from '@material-ui/core/CircularProgress';
import PropTypes from "prop-types";


export default function Asynchronous({srcUrl, label}) {
    const [open, setOpen] = useState(false);
    const [options, setOptions] = useState([]);
    const loading = open && options.length === 0;

    useEffect(() => {
        let active = true;

        if (!loading) {
            return undefined;
        }

        (async () => {
            const response = await axios.get(srcUrl);
            const data = response.data.length < 7 ? response.data : response.data.slice(0, 8);

            if (active) {
                setOptions(Object.keys(data).map((key) => data[key]));
            }
        })();

        return () => {
            active = false;
        };
    }, [loading, srcUrl]);

    useEffect(() => {
        if (!open) {
            setOptions([]);
        }
    }, [open]);

    const searchAirports = async (event) => {
        const value = event.target.value;

        if (value === undefined || value.length < 3)
            return;

        const response = await axios.get(`${srcUrl}?search=${value}`);
        const data = response.data.length < 7 ? response.data : response.data.slice(0, 8);

        setOptions(Object.keys(data).map((key) => data[key]));
    };

    // console.log(options);

    return (
        <Autocomplete
            open={open}
            onOpen={() => {
                setOpen(true);
            }}
            onClose={() => {
                setOpen(false);
            }}
            getOptionSelected={(option, value) => option.name.search(value.name) > 0 }
            getOptionLabel={(option) => option.name}
            options={options}
            loading={loading}
            onInputChange={searchAirports}
            renderInput={(params) => (
                <TextField
                    {...params}
                    label={label}
                    variant="outlined"
                    InputProps={{
                        ...params.InputProps,
                        endAdornment: (
                            <Fragment>
                                {loading ? <CircularProgress color="inherit" size={20}/> : null}
                                {params.InputProps.endAdornment}
                            </Fragment>
                        ),
                    }}
                />
            )}
        />
    );
}

Asynchronous.propTypes = {
    srcUrl: PropTypes.string.isRequired,
    label: PropTypes.string.isRequired
};
