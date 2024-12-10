import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";
import "./styles.css"
import { Link } from "react-router-dom";

export default observer (function NavBar() {
    const { userStore } = useStore();
    return (
        <AppBar sx={{ bgcolor: "primary.main", p:"0px 50px" }}>
            <Toolbar sx={{ display: "flex", flexDirection: "row", gap: "20px" }}>
                <Box display="flex" flexDirection="row" width="70%" gap="30px" justifyContent="flex-start" alignItems="center">
                    <Link to="/" >
                        <Typography variant="body1" color="text.primary" className="navLink">Absolute cinema</Typography>
                    </Link>
                    <Link to="/cinema">
                        <Typography variant="body1" color="text.primary" className="navLink">Cinemas</Typography>
                    </Link>
                    <Link to="/session">
                        <Typography variant="body1" color="text.primary" className="navLink">Sessions</Typography>
                    </Link>
                    <Link to="/movie">
                        <Typography variant="body1" color="text.primary" className="navLink">Movies</Typography>
                    </Link>
                </Box>
                {userStore.isLoggedIn ? (
                    <Box display="flex" flexDirection="row" width="30%" justifyContent="flex-end" gap="25px" alignItems="center">
                        <Typography variant="body1" color="text.primary">Hello, {userStore.getUser()?.name}!</Typography>
                        <Button variant="outlined" color="primary" onClick={() => { userStore.logOut(); }}>Logout</Button>
                    </Box>
                    ) : (
                    <Box display="flex" flexDirection="row" width="30%" justifyContent="flex-end" gap="25px">
                        <Button variant="outlined" color="primary" onClick={() => { userStore.logIn({ email: "admin@gmail.com", password:"admin" }) }}>Login</Button>
                        <Button variant="outlined" color="primary">Register</Button>
                    </Box>
                )}
            </Toolbar>
        </AppBar>
    )
})