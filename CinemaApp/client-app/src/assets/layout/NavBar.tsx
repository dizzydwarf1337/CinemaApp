import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";
import "./styles.css"
import { Link } from "react-router-dom";
import CustomSnackBar from "./CustomSnackBar";
import LocalActivityIcon from '@mui/icons-material/LocalActivity';
import LoginModal from "../../features/user/loginModal";
import { useState } from "react";
import RegisterModal from "../../features/user/registerModal";


export default observer (function NavBar() {
    const { userStore } = useStore(); 
    const [openLoginModal, setOpenLoginModal] = useState(false);
    const [openRegisterModal,setOpenRegisterModal] = useState(false);
    return (
        <>
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
                            <Link to="/myTickets">
                                <LocalActivityIcon color="secondary" sx={{ ":hover":"color:"}} />
                            </Link>
                            <Typography variant="body1" color="text.primary">Hello, {userStore.getUser()?.name}!</Typography>
                            <Button variant="outlined" color="primary" onClick={() => {userStore.logOut();}}>Logout</Button>
                        </Box>
                        ) : (
                            <Box display="flex" flexDirection="row" width="30%" justifyContent="flex-end" gap="25px">
                                <Button variant="outlined" color="primary" onClick={() => setOpenLoginModal(true)}>Login</Button>
                                <Button variant="outlined" color="primary" onClick={()=> setOpenRegisterModal(true) }>Register</Button>
                        </Box>
                    )}
                </Toolbar>
            </AppBar>
            <CustomSnackBar
                message="You have been logged out"
                severity="success"
                open={userStore.getUserLogOutSnack()}
                onClose={()=>userStore.setUserLogOutSnack(false)}
            />
            <CustomSnackBar
                message="You logged in successfully"
                severity="success"
                open={userStore.getUserLogInSnack()}
                onClose={() => userStore.setUserLogInSnack(false)}
            />
            <LoginModal open={openLoginModal} onClose={() => setOpenLoginModal(false)} />
            <RegisterModal open={openRegisterModal} onClose={() => setOpenRegisterModal(false)} />
        </>
    )
})