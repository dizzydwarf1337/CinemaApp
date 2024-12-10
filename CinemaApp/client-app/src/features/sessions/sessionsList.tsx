import { Box, FormControl, InputLabel, MenuItem, Select, SelectChangeEvent, TextField, Typography } from "@mui/material";
import SessionListItem from "./sessionsListItem";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";
import { useEffect, useMemo, useState } from "react";
import { DatePicker, LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs, { Dayjs } from "dayjs";

export default observer(function SessionList() {

    const { cinemaStore, userStore } = useStore();
    const [selectedGenre, setSelectedGenre] = useState<string>("All");
    const uniqueGenres = useMemo(() =>
        ["All", ...new Set(cinemaStore.movies.map(movie => movie.genre))],
        [cinemaStore.movies]
    );
    const [selectedDate, setSelectedDate] = useState<Dayjs | null>(dayjs());
    const [selectedMovie, setSelectedMovie] = useState<string>("All");
    const [selectedCinema, setSelectedCinema] = useState<string>("All");
    const [filteredSessions, setFilteredSessions] = useState(cinemaStore.sessions);

    const handleChangeGenre = (event: SelectChangeEvent) => {
        setSelectedGenre(event.target.value);
    };
    const handleChangeDate = (newValue: Dayjs | null) => {
        if (newValue) setSelectedDate(dayjs(newValue));
        else setSelectedDate(dayjs());
    };
    const handleChangeMovie = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSelectedMovie(event.target.value);
    };
    const handleChangeCinema = (event: SelectChangeEvent<string>) => {
        setSelectedCinema(event.target.value);
    };

    useEffect(() => {
        const filtered = cinemaStore.sessions
            .filter(session => {
                const sessionDate = dayjs(session.date);

                const isValidDate = userStore.user?.role === 'admin' || sessionDate.isAfter(selectedDate?.isBefore(dayjs()) ? dayjs() : selectedDate, 'd');
                const matchesGenre = selectedGenre === 'All' ||
                    (cinemaStore.movies.some(movie => movie.id === session.movieId && movie.genre === selectedGenre));

                const matchesCinema = selectedCinema === 'All' ||
                    (cinemaStore.halls.some(hall => hall.id === session.hallId &&
                        cinemaStore.cinemas.some(cinema => cinema.id === hall.cinemaId && cinema.name === selectedCinema)));

                const matchesMovie = selectedMovie === 'All' ||
                    cinemaStore.movies.some(movie => movie.id === session.movieId && movie.title.toLowerCase().includes(selectedMovie.toLowerCase()));

                return isValidDate && matchesGenre && matchesCinema && matchesMovie;
            })
            .sort((session1, session2) => {
                const date1 = dayjs(session1.date);
                const date2 = dayjs(session2.date);
                return date1.isBefore(date2) ? -1 : 1;
            });

        setFilteredSessions(filtered);
    }, [cinemaStore.sessions, selectedGenre, selectedDate, selectedMovie, selectedCinema, cinemaStore, userStore.user]);

    return (
        <Box display="flex" flexDirection="column" gap="10px">
            <Box display="flex" flexDirection="row" borderRadius="8px" justifyContent="space-evenly" sx={{ boxShadow: "2px 2px 2px 2px grey", p: "20px", backgroundColor: "#DEDFDF", alignItems: "center" }}>
                <Typography variant="body1" color="black">Filters:</Typography>
                <Box display="flex" flexDirection="row" gap="10px" justifyContent="flex-start">
                    <FormControl>
                        <Box display="flex" flexDirection="row" width="170px">
                            <InputLabel id="genre-label">Genre</InputLabel>
                            <Select labelId="genre-label" id="genre-select" value={selectedGenre} onChange={handleChangeGenre} label="Genre" fullWidth
                                inputProps={{ id: "genre-native-input", name: "genre" }}>
                              
                                {uniqueGenres.map(genre => (
                                    <MenuItem id={genre} key={genre} value={genre}>
                                        <Typography color="black">{genre}</Typography>
                                    </MenuItem>
                                ))}
                            </Select>
                        </Box>
                    </FormControl>
                </Box>
                <Box>
                    <FormControl id="cinema-form">
                        <Box display="flex" flexDirection="row" width="170px">
                            <InputLabel id="cinema-label">Cinema</InputLabel>
                            <Select labelId="cinema-label" id="cinema-select" value={selectedCinema} onChange={handleChangeCinema} label="Cinema" fullWidth
                                inputProps={{ id: "cinema-native-input", name: "cinema" }}>
                                <MenuItem id={'all'} key='All' value='All'>
                                    <Typography color="black">All</Typography>
                                </MenuItem>
                                {cinemaStore.cinemas.map(cinema => (
                                    <MenuItem key={cinema.id} value={cinema.name}>
                                        <Typography color="black">{cinema.name}</Typography>
                                    </MenuItem>
                                ))}
                            </Select>
                        </Box>
                    </FormControl>
                </Box>
                <Box>
                    <FormControl>
                        <Box display="flex" flexDirection="row" width="170px">
                            <LocalizationProvider dateAdapter={AdapterDayjs}>
                                <DatePicker value={selectedDate} onChange={handleChangeDate} label="Date"
                                    slotProps={{
                                        layout: {
                                            sx: {
                                                color: 'white',
                                                borderRadius: '2px',
                                                borderWidth: '1px',
                                                borderColor: '#e91e63',
                                                border: '1px solid',
                                                backgroundColor: '#1a202c'
                                            }
                                        },
                                        textField: {
                                            sx: {
                                                '& .MuiInputBase-input': {
                                                    color: 'black'
                                                }
                                            }
                                        }
                                    }} />
                            </LocalizationProvider>
                        </Box>
                    </FormControl>
                </Box>
                <Box>
                    <FormControl>
                        <Box display="flex" flexDirection="row" width="170px">
                            <TextField
                                value={selectedMovie}
                                onChange={handleChangeMovie}
                                id="movie-select"
                                label="Movie"
                                placeholder="Select Movie"
                                sx={{
                                    '& .MuiInputBase-input': {
                                        color: 'black'
                                    }
                                }}
                            />
                        </Box>
                    </FormControl>
                </Box>
            </Box>
            {filteredSessions.map(session => (
                <Box key={session.id}>
                    <SessionListItem session={session} />
                </Box>
            ))}
        </Box>
    );
});
