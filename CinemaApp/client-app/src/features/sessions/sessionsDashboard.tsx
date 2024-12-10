import { Box, Button, Typography } from "@mui/material";
import { useStore } from "../../assets/stores/store";
import SessionList from "./sessionsList";
import { observer } from "mobx-react-lite";


export default observer(function SessionsDashboard() {

    const { userStore } = useStore();
    return (
        <>
            <Box sx={{ display: "flex", flexDirection: "column", height: "100%", m: "100px", gap: "20px" }}>
                <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "space-between", alignItems: "center" }} >
                    <Typography variant="h3" color="primary">{userStore.user?.role==='admin'? "All sessions" : "Future sessions"}</Typography>
                    {userStore.user && userStore.user.role === "admin" && (
                        <Button variant="contained" color="success">Add session</Button>
                    )}
                </Box>
                <SessionList/>
            </Box>
        </>
    )
})