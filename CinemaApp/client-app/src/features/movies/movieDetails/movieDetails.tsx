import { observer } from "mobx-react-lite"
import { useParams } from "react-router";
import { useStore } from "../../../assets/stores/store";
import Movie from "../../../assets/models/movie";
import { useEffect, useState } from "react";
import dayjs from "dayjs";
import { Box, Typography } from "@mui/material";
import SessionsListItem from "../../sessions/sessionsListItem";

export default observer(function MovieDetails() {

    const { cinemaStore } = useStore();
    const { id } = useParams<{id:string}>();
    const [movie, setMovie] = useState<Movie | null>(null);
    useEffect(() => {
        const loadMovie = async () => {
            let foundMovie = cinemaStore.movies.find((movie) => movie.id === id);
            if (!foundMovie && id) {
                foundMovie = await cinemaStore.getMovieApi(id);
            }
            setMovie(foundMovie || null);
        };
        loadMovie();
    }, [cinemaStore, id]);

    const sessions = cinemaStore.sessions.filter((session) => session.movieId === id && dayjs(session.date).isAfter(dayjs()));


    return (
        <>
        <Box display="flex" flexDirection="column"  m="100px" gap="20px">
                <Box display="flex" flexDirection="column">
                    <Box display="flex" flexDirection="row" width="100%"  justifyContent="space-between">
                        <Box display="flex" flexDirection="column" justifyContent="flex-start">
                            <Box sx={{
                                borderRadius: "15px",
                                boxShadow:"2px 2px 2px grey",
                                backgroundImage: movie?.imagePath ? `url(${movie.imagePath})` : 'url(/absoluteCinema.jpg)', width: "500px", height: "400px", backgroundSize: "cover",
                                backgroundPosition: "center",
                                backgroundRepeat: 'no-repeat',
                                }} />
                        </Box>
                        <Box display="flex" flexDirection="column"  justifyContent="center">
                            <Typography variant="h2" color="primary">{movie?.title}</Typography>
                            <Box display="flex" flexDirection="row" alignItems="center" gap="20px">
                                <Typography variant="h3" color="secondary" sx={{ textShadow:"1px 1px 1px #1a202c"} } >{movie?.genre}</Typography>
                                <Typography variant="h3" color="secondary" sx={{ textShadow:"1px 1px 1px #1a202c"} } >{movie?.duration}</Typography>
                            </Box>
                            <Box>
                                <Typography variant="h6" color="primary">{movie?.description}</Typography>
                                <Typography variant="h6" color="secondary" sx={{ textShadow: "1px 1px 1px #1a202c" }}>By {movie?.director}</Typography>
                            </Box>
                        </Box>
                    </Box>
                </Box>
                <Box>
                    {sessions.length > 0 ? (
                        <>
                        <Typography variant="h4">Sessions</Typography>
                            <Box display="flex" flexDirection="column" gap="20px">
                            {sessions.map(session => (
                                <Box key={session.id}>
                                    <SessionsListItem session={session} />
                                </Box>
                            ))}
                        </Box>
                        </>
                    ) : (
                            <>
                                <Typography variant="h3" color="error">No sessions Available</Typography>
                            </>
                    )}
                </Box>
            </Box>
        </>
    )
})