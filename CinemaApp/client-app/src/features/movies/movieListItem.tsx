import { Box, Button, Card, CardActions, CardContent, CardHeader, CardMedia, Typography } from "@mui/material";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";
import movie from "../../assets/models/movie";
import { useNavigate } from "react-router";

interface Props {
    movie: movie;
}

export default observer(function cinemaListItem({ movie }: Props) {

    const { userStore, cinemaStore } = useStore();
    const navigate = useNavigate();
    return (
        <>
            <Box width="100%" height="100%" onClick={() => { navigate(`/movie/${movie.id}`) } }>
                <Card sx={{
                    height: userStore.user?.role === 'admin' ? "430px" : "390",
                    width: "310px",
                    borderRadius: "15px"
                }}>
                    <CardHeader sx={{ color: "#1a202c", backgroundColor: "white", height: "10px" }} title={movie.title}></CardHeader>
                    <CardMedia component="img" image={movie.imagePath ? `${movie.imagePath}` : '/absoluteCinema.jpg'}
                        sx={{
                            height: "290px",
                            backgroundSize: "cover",
                            backgroundPosition: "center",
                            backgroundRepeat: 'no-repeat'
                        }} ></CardMedia>
                    <CardContent sx={{ color: 'white', height: "10px", backgroundColor: "primary.main" }}>
                        <Box display="flex" flexDirection="row" justifyContent="space-between">
                            <Typography sx={{ color: "#f4b400", textShadow: "1px 1px  #1a202c" }}>{movie.genre}</Typography>
                            <Typography sx={{ color: "#f4b400", textShadow: "1px 1px  #1a202c" }}>{movie.director}</Typography>
                        </Box>
                    </CardContent>
                    {userStore.user?.role === 'admin' ? (
                        <CardActions >
                            <Box width="100%" display="flex" flexDirection="row" justifyContent="flex-end" gap="20px">
                                <Button variant="contained" color="warning">Edit</Button>
                                <Button variant="contained" onClick={() => { cinemaStore.deleteMovie(movie.id) }} color="error">Delete</Button>

                            </Box>
                        </CardActions>
                    ) : null
                    }

                </Card>
            </Box>
        </>
    )
})