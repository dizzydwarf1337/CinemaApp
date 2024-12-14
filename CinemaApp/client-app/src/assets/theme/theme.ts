import { createTheme } from '@mui/material/styles';

const theme = createTheme({
    palette: {
        primary: {
            main: '#1a202c', //  Raisin black (blue)
            light: "#667FB1"
        },
        secondary: {
            main: '#f4b400', //  Selective yellow
        },
        background: {
            default: '#121212', // Night
        },
        success: {
            main: '#34d399', // Emerald (green)
        },
        error: {
            main: '#e11d48', // Amaranth red
        },
        warning: {
            main: '#f59e0b', // Gambodge (orange)
        },
        text: {
            primary: '#ffffff', // White 
            secondary: '#9ca3af', // Cadet grey (bluish grey)
        },
    },
    components: {
        MuiButton: {
            styleOverrides: {
                root: {
                    textDecoration: "none",
                    border: "none",
                    color: "#ffffff",
                    textTransform: "none",
                    borderRadius: "7px",
                    fontSize: "1rem",
                },
                outlinedPrimary: {
                    boxShadow: "0px 0px 2.5px 1.5px #f4b400",
                    transition: "all 0.3s ease",
                    '&:hover': {
                        boxShadow: "none",
                        backgroundColor: "#f4b400",
                        color: "#1a202c",
                    },
                },
                containedSuccess: {
                    backgroundColor: "#619129",

                    '&:hover': {
                        backgroundColor: "#74AC33",
                    }
                }
            },
        },
        MuiTextField: {
            styleOverrides: {
                root: {

                    '& .MuiInputBase-input': {
                        color: 'black',
                    },
                    '& .MuiFormLabel-root': {
                        color: 'black',
                    },
                },
            },
        },
    }
});

export default theme;
