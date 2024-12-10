import { Box, Button, Typography } from "@mui/material";
import CinemasList from "./cinemasList";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";

export default observer(function CinemaDashboard() {
    const { userStore } = useStore();
    const navigate = useNavigate();
    return (
        <>
            <Box sx={{
                display: "flex",
                flexDirection: "column",
                height: "100%",
                m: { xs: "20px", sm: "40px", md: "100px" },
                gap: "20px"
            }}>
                <Box sx={{
                    display: "flex",
                    flexDirection: { xs: "column", sm: "row" },
                    justifyContent: "space-between",
                    alignItems: "center"
                }}>
                    <Typography variant="h3" color="primary" sx={{ fontSize: { xs: "1.5rem", sm: "2rem", md: "3rem" } }}>
                        Our cinemas
                    </Typography>
                    {userStore.user && userStore.user.role === "admin" && (
                        <Button variant="contained" onClick={() => {navigate(`/cinema/manage`)} } color="success" sx={{ mt: { xs: "10px", sm: "0" } }}>
                            Add Cinema
                        </Button>
                    )}
                </Box>
                <CinemasList />
            </Box>
        </>
    );
});
