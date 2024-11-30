import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";


export default observer (function NavBar() {
    const { userStore } = useStore();
    return (
        <AppBar sx={{ bgcolor: "primary.main", p:"0px 50px" }}>
            <Toolbar sx={{ display: "flex", flexDirection: "row", gap: "20px" }}>
                <Box display="flex" flexDirection="row" width="70%" gap="30px" justifyContent="flex-start" alignItems="center">
                    <Typography variant="body1" color="text.primary">Absolute cinema</Typography>
                    <Typography variant="body1" color="text.primary">Cinemas</Typography>
                    <Typography variant="body1" color="text.primary">Sessions</Typography>
                    <Typography variant="body1" color="text.primary">Movies</Typography>
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