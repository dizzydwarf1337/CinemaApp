import { observer } from "mobx-react-lite";
import { Form, useParams } from "react-router";
import { useStore } from "../../../assets/stores/store";
import Session from "../../../assets/models/session";
import { useEffect, useState } from "react";
import { Box, Button, FormControl, InputLabel, MenuItem, Select, SelectChangeEvent, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router";
import CustomSnackbar from "../../../assets/layout/CustomSnackBar";

export default observer(function SessionForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { cinemaStore, userStore } = useStore();
    const [sessionData, setSessionData] = useState<Session>({
        id: "00000000-0000-0000-0000-000000000000",
        date: new Date(),
        movieId: "00000000-0000-0000-0000-000000000000",
        hallId: "00000000-0000-0000-0000-000000000000",
        ticketPrice: 0.0,
        availibleSeats: 0,
    });

    useEffect(() => {
        if (id) {
            const session = cinemaStore.sessions.find((session) => session.id === id);
            if (session) setSessionData(session);
        } else {
            setSessionData((prev) => ({
                ...prev,
                movieId: cinemaStore.movies[0].id,
                hallId: cinemaStore.halls[0].id,
            }));
        }
    }, [id, cinemaStore.sessions, cinemaStore.movies, cinemaStore.halls]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setSessionData({ ...sessionData, [name]: value });
    };

    const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const dateValue = new Date(e.target.value);
        setSessionData({ ...sessionData, date: dateValue });
    };

    const handleSelectChange = (e: SelectChangeEvent<string>) => {
        const { name, value } = e.target;
        setSessionData({ ...sessionData, [name]: value });
    };

    const handleSubmit = async () => {
        try {
            if (id) {
                await cinemaStore.updateSession(sessionData);
            } else {
                await cinemaStore.createSession(sessionData);
            }
        } catch (error) {
            console.error("Error submitting session data:", error);
        }
    };

    const userRole = userStore.getUser()?.role;
    if (userRole !== 'admin') {
        return (
            <Box position="absolute" display="flex" justifyContent="center" alignItems="center" width="100%" height="100%">
                <Typography variant="h1" color="error">Forbidden</Typography>
            </Box>
        );
    }

    return (
        <>
        <Box display="flex" flexDirection="column" bgcolor="#DEDFDF" boxShadow="2px 2px 2px 2px grey" width="500px" height="auto" m="100px" borderRadius="10px" alignSelf="center" justifySelf="center">
            <Box display="flex" justifyContent="center" alignItems="center" mt="20px">
                <Typography color="secondary" variant="h6" sx={{ textShadow: "1px 1px 0.1px #1a202c" }}>
                    {id ? 'Update Session' : 'Create Session'}
                </Typography>
            </Box>
            <Form onSubmit={handleSubmit}>
                <Box display="flex" flexDirection="column" m="20px" gap="20px" height="100%" justifyContent="space-between">
                    <FormControl fullWidth>
                        <TextField
                            required
                            type="datetime-local"
                            value={new Date(sessionData.date).toISOString().slice(0, 16)}
                            name="date"
                            onChange={handleDateChange}
                            label="Session Date"
                        />
                    </FormControl>
                    <FormControl>
                        <InputLabel id="movie-lable">Movie</InputLabel>
                        <Select labelId="movie-lable" id="select-movie" name="movieId" onChange={handleSelectChange} label="Movie" value={sessionData.movieId} required>
                            {cinemaStore.movies.map(movie => (
                                <MenuItem key={movie.id} value={movie.id}>
                                    <Typography color="black">{movie.title}</Typography>
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                    <FormControl>
                        <InputLabel id="hall-lable">Hall</InputLabel>
                        <Select labelId="hall-lable" id="select-hall" name="hallId" onChange={handleSelectChange} label="Hall" value={sessionData.hallId} required>
                            {cinemaStore.halls
                                .map(hall => {
                                    const cinema = cinemaStore.cinemas.find(cinema => cinema.id === hall.cinemaId);
                                    return { hall, cinema: cinema?.name || '' };
                                })
                                .sort((a, b) => a.cinema.localeCompare(b.cinema) || a.hall.number - b.hall.number)
                                .map(({ hall, cinema }) => (
                                    <MenuItem key={hall.id} value={hall.id}>
                                        <Typography color="black">{`${cinema} Hall: ${hall.number}`}</Typography>
                                    </MenuItem>
                                ))}
                        </Select>
                    </FormControl>
                    <FormControl>
                        <TextField
                            value={sessionData.ticketPrice}
                            name="ticketPrice"
                            type="number"
                            onChange={handleInputChange}
                            label="Ticket Price"
                            required
                        />
                    </FormControl>
                    <FormControl>
                        <TextField
                            value={sessionData.availibleSeats}
                            name="availibleSeats"
                            type="number"
                            onChange={handleInputChange}
                            label="Available Seats"
                            required
                        />
                    </FormControl>
                    <FormControl>
                        <Button type="submit" variant="contained" color={id ? "warning" : "success"}>
                            {id ? "Update Session" : "Create Session"}
                        </Button>
                    </FormControl>
                </Box>
            </Form>
            </Box>
            <CustomSnackbar
                open={cinemaStore.getCreateSnack()}
                message={"Session has been created. You will be redirected"}
                severity={"success"}
                onClose={() => { cinemaStore.setCreateSnack(false); navigate('/session') }} />
            <CustomSnackbar
                open={cinemaStore.getUpdateSnack()}
                message={"Session has been updated. You will be redirected"}
                severity={"warning"}
                onClose={() => { cinemaStore.setUpdateSnack(false); navigate('/session') }} />
        </>
    );
});
