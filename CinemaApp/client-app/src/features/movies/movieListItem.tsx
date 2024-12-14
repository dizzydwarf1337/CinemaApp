import { Box, Button, Card, CardContent, CardHeader, Typography } from "@mui/material";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";
import movie from "../../assets/models/movie";
import { useNavigate } from "react-router";
import CustomSnackbar from "../../assets/layout/CustomSnackBar";

interface Props {
    movie: movie;
}

export default observer(function movieListItem({ movie }: Props) {

    const { userStore, cinemaStore } = useStore();
    const navigate = useNavigate();

    return (
        <>
            <Box width="100%" height="100%" onClick={() => { navigate(`/movie/${movie.id}`) }} sx={{ cursor: "pointer" }}>
                <Card sx={{
                    backgroundImage: movie.imagePath ? `url(${movie.imagePath})` : 'url(/absoluteCinema.jpg)',
                    backgroundSize: "cover",
                    backgroundPosition: "center",
                    backgroundRepeat: 'no-repeat',
                    height: "450px", 
                    width: "310px",
                    borderRadius: "15px",
                    position: 'relative',
                }}>
                    <CardHeader
                        sx={{
                            position: 'absolute',
                            top: '0px',
                            left: '10px',
                            color: "white",
                            textShadow: "2px 2px 2px red",
                        }}
                       
                    />
                    <CardContent sx={{
                        position: 'absolute',
                        bottom: '0px',
                        left: '10px',
                        right: '10px',
                        color: '#ffb400',
                    }}>
                       
                    </CardContent>

                </Card>
               
                    <Box display="flex" flexDirection="row" justifyContent="space-between">
                        <Box sx={{ display: 'flex', justifyContent: 'flex-start', mt: "5px", mr: "15px", gap: "10px" }}>
                        <Typography variant="body1" color="secondary" sx={{ textShadow:"1px 1px 1px #1a202c"} }>{movie.title}</Typography>
                        </Box>
                    {userStore.getUser()?.role === 'admin' && (
                        <Box sx={{ display: 'flex', mt: "5px", mr: "15px", gap: "10px" }}>
                            <Button variant="contained" color="warning" onClick={(e) => { e.stopPropagation(); navigate(`/movie/manage/${movie.id}`) }}>Edit</Button>
                            <Button variant="contained" onClick={(e) => { e.stopPropagation(); cinemaStore.deleteMovie(movie.id) }} color="error">Delete</Button>
                        </Box>
                    )}
                    </Box>
                
            </Box>
            <CustomSnackbar
                message="Movie deleted"
                open={cinemaStore.getDeleteSnack()}
                autoHideDuration={3000}
                key="DeleteSnackBar"
                severity="error"
                onClose={() => cinemaStore.setDeleteSnack(false)}
            />
        </>
    );
});
