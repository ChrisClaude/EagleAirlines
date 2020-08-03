import axios from "axios";
import React, { Fragment, useEffect, useState } from 'react';
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
            const data = response.data;

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
            getOptionSelected={(option, value) => option.name === value.name}
            getOptionLabel={(option) => option.name}
            options={options}
            loading={loading}
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
