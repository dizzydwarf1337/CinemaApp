import { Box, Button, Card, CardActions, CardContent, CardHeader, CardMedia, Typography } from "@mui/material";
import cinema from "../../assets/models/cinema";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";

interface Props {
    cinema: cinema;
}

export default observer(function cinemaListItem({ cinema }: Props) {

    const { userStore, cinemaStore } = useStore();
    const navigate = useNavigate();
    return (
        <>
            <Box width="100%" height="100%" sx={{ cursor: "pointer" }}>
                <Card onClick={() => {navigate(`/cinema/${cinema.id}`)} } sx={{
                    height: userStore.user?.role === 'admin' ? "430px" : "390",
                    width: "310px",
                    borderRadius: "15px",
                }}>
                    <CardHeader sx={{ color: "#1a202c", backgroundColor:"white",height:"10px" }} title={cinema.name}></CardHeader>
                    <CardMedia component="img" image={cinema.imagePath ? `${cinema.imagePath}` : '/absoluteCinema.jpg'}
                        sx={{ 
                        height:"290px",
                        backgroundSize: "cover",
                        backgroundPosition: "center",
                        backgroundRepeat: 'no-repeat'
                        }} ></CardMedia>
                    <CardContent sx={{ color: 'white', height:"10px", backgroundColor:"primary.main" }}>
                        <Box display="flex" flexDirection="column">
                            <Typography sx={{ color: "#f4b400", textShadow: "1px 1px  #1a202c" }}>{cinema.address}</Typography>
                        </Box>
                    </CardContent>
                    {userStore.user?.role === 'admin' ? (
                        <CardActions >
                            <Box width="100%" display="flex" flexDirection="row" justifyContent="flex-end" gap="20px">
                                <Button variant="contained" onClick={(e) => { e.stopPropagation(); navigate(`/cinema/manage/${cinema.id}`) }} color="warning">Edit</Button>
                                <Button variant="contained" onClick={(e) => { e.stopPropagation(); cinemaStore.deleteCinema(cinema.id) }} color="error">Delete</Button>

                            </Box>
                        </CardActions>
                    ) : null
                    }

                </Card>
            </Box>
        </>
    )
})