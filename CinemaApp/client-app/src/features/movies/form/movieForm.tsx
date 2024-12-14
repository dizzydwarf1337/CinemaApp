import { observer } from "mobx-react-lite";
import { useStore } from "../../../assets/stores/store";
import { Form, useParams } from "react-router";
import { ChangeEvent, useEffect, useState } from "react";
import { Box, Button, FormControl, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router";
import Movie from "../../../assets/models/movie";
import CustomSnackbar from "../../../assets/layout/CustomSnackBar";

export default observer(function MovieForm() {
    const { cinemaStore, userStore } = useStore();
    const { id } = useParams();
    const navigate = useNavigate();
    const [movieData, setMovieData] = useState<Movie>({
        id: "00000000-0000-0000-0000-000000000000",
        title: "",
        description: "",
        genre: "",
        director: "",
        duration: "",
        imagePath: "",
    });
    const [photo, setPhoto] = useState<File>();
    useEffect(() => {
        if (id) {
            const movie = cinemaStore.movies.find((movie) => movie.id === id);
            if (movie) setMovieData({ ...movie, imagePath: "" });
        }
    }, [id, cinemaStore]);

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setMovieData({ ...movieData, [name]: value });
    };
    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setPhoto(event.target.files[0]);
        }
    };

    const handleSubmit = (event: ChangeEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (id) {
            cinemaStore.updateMovie(movieData, photo);
        } else {
            cinemaStore.createMovie(movieData, photo);
        }
    };
    if (userStore.getUser()?.role === 'admin')
        return (
        <>
            <Box display="flex" flexDirection="column" bgcolor="#DEDFDF" boxShadow="2px 2px 2px 2px grey" width="500px" height="auto" m="100px" borderRadius="10px" alignSelf="center" justifySelf="center">
                <Box display="flex" justifyContent="center" alignItems="center" mt="20px">
                    <Typography color="secondary" variant="h6" sx={{ textShadow: "1px 1px 0.1px #1a202c" }}>{id ? 'Update Movie' : 'Create Movie'}</Typography>
                </Box>
                <Form onSubmit={handleSubmit}>
                    <Box display="flex" flexDirection="column" m="20px" gap="20px" height="100%" justifyContent="space-between">
                        <FormControl sx={{ color: "black" }}>
                            <TextField
                                value={movieData.title}
                                name="title"
                                onChange={handleInputChange}
                                label="Movie title"
                                required
                            />
                        </FormControl>
                        <FormControl>
                            <TextField
                                value={movieData.description}
                                name="description"
                                onChange={handleInputChange}
                                label="Description"
                                required
                            />
                        </FormControl>
                        <FormControl>
                        <TextField
                                value={movieData.genre}
                                onChange={handleInputChange}
                                name="genre"
                                label="Genre"
                                required
                            />
                        </FormControl>
                        <FormControl>
                            <TextField
                                value={movieData.director}
                                onChange={handleInputChange}
                                name="director"
                                label="Director"
                                required
                            />
                        </FormControl>
                        <FormControl>
                            <TextField
                                type="time"
                                value={ movieData.duration}
                                onChange={handleInputChange}
                                name="duration"
                                required
                            />
                        </FormControl>
                        <FormControl>
                            <TextField
                                type="file"
                                onChange={handleFileChange}
                                name="photo"
                            />
                        </FormControl>
                        <FormControl>
                            <Button type="submit" variant="contained" color={id ? "warning" : "success"}>
                                {id ? "Update Movie" : "Create Movie"}
                            </Button>
                        </FormControl>
                    </Box>
                </Form>
                </Box>
                <CustomSnackbar
                    open={cinemaStore.getCreateSnack()}
                    message={ "Movie has been created. You will be redirected"}
                    severity={"success"}
                    onClose={() => { cinemaStore.setCreateSnack(false); navigate('/movie') }} />
                <CustomSnackbar
                    open={cinemaStore.getUpdateSnack()}
                    message={"Movie has been updated. You will be redirected" }
                    severity={"warning"}
                    onClose={() => { cinemaStore.setUpdateSnack(false); navigate('/movie') }} />
                
            </>
        );
    else return (
        <Box position="absolute" display="flex" justifyContent="center" alignItems="center" width="100%" height="100%">
            <Typography variant="h1" color="error">Forbidden</Typography>
        </Box>
    )
});
