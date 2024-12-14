import { Box, Button, Typography } from "@mui/material";
import { useStore } from "../../assets/stores/store";
import { useEffect, useMemo, useState } from "react";
import Movie from "../../assets/models/movie";
import Cinema from "../../assets/models/cinema";
import Hall from "../../assets/models/hall";
import Session from "../../assets/models/session";
import { Link } from "react-router";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";
import CustomSnackbar from "../../assets/layout/CustomSnackBar";
import PurchaseTicketModal from "../tickets/purhcaseTicketModal";

interface Props {
    session: Session;
}

export default observer(function SessionListItem({ session }: Props) {
    const { cinemaStore, userStore } = useStore();
    const [movie, setMovie] = useState<Movie | undefined>();
    const [cinema, setCinema] = useState<Cinema | undefined>();
    const [hall, setHall] = useState<Hall | undefined>();
    const [openModal, setOpenModal] = useState(false);
    const navigate = useNavigate();
    useEffect(() => {
        const foundHall = cinemaStore.halls.find(hall => hall.id === session.hallId);
        setHall(foundHall);

        const foundMovie = cinemaStore.movies.find(movie => movie.id === session.movieId);
        setMovie(foundMovie);

        if (foundHall) {
            const foundCinema = cinemaStore.cinemas.find(cinema => cinema.id === foundHall.cinemaId);
            setCinema(foundCinema);
        }
    }, [cinemaStore, session.hallId, session.movieId]);

    const sessionDate = useMemo(() => {
        return new Date(session.date).toLocaleString('pl-PL', {
            year: "numeric",
            month: "numeric",
            day: "numeric",
            hour: "numeric",
            minute: "numeric",
            hour12: false,
        });
    }, [session.date]);

    return (
        <>
            <Box display="flex" flexDirection="row" sx={{
                borderRadius: "7px",
                boxShadow: "2px 2px 2px 2px grey",
                height: "auto",
                p: "10px 30px 10px 30px",
                backgroundColor: "#DEDFDF",
            }}>
                <Box display="flex" flexDirection="column" justifyContent="center" alignItems="flex-start" flex="2" gap="5px">
                    <Link to={`/movie/${movie?.id}` }>
                        <Typography variant="h5" color="secondary" textAlign="start" sx={{ textShadow: "1px 1px 1px #1a202c" }}>
                            {movie?.title || "No movie found"}
                        </Typography>
                    </Link>
                    <Typography variant="body2">
                        {movie?.description || "No description found"}
                    </Typography>
                </Box>

                <Box display="flex" flexDirection="column" justifyContent="center" alignItems="flex-start" flex="1" gap="5px">
                    <Link to={`/cinema/${cinema?.id}` }>
                    <Typography variant="body1" color="primary" sx={{ fontWeight: "bold", textShadow: "2px 1px 1px #f4b400" }}>
                        {cinema?.name || "No cinema found"}
                        </Typography>
                    </Link>
                    <Typography variant="body2">
                        Genre: {movie?.genre || "No genre found"}
                    </Typography>
                </Box>

                <Box display="flex" flexDirection="column" justifyContent="center" alignItems="flex-start" flex="1" gap="5px">
                    <Typography variant="body1" color="primary.main">
                        {`Hall ${hall?.number}` || "No hall found"}
                    </Typography>
                    <Typography variant="body2">
                        Director: {movie?.director || "No directors found"}
                    </Typography>
                </Box>

                <Box display="flex" flexDirection="column" justifyContent="center" alignItems="flex-start" flex="1" gap="5px">
                    <Typography variant="body1">
                        {sessionDate || "No datetime found"}
                    </Typography>
                    <Typography variant="body2">
                        Remaining seats: {session.availibleSeats}
                    </Typography>
                </Box>

                <Box display="flex" justifyContent="center" alignItems="center" flexDirection="column" gap="5px">
                    <Button variant="contained" color="secondary" onClick={() => { userStore.getUser() ? setOpenModal(true) : navigate('/login'); }}>Purchase ticket</Button>
                    {userStore.user?.role === "admin" && (
                        <Box display="flex" flexDirection="row" gap="4px">
                            <Button variant="contained" color="warning" onClick={() => { navigate(`/session/manage/${session.id}`) }}>Edit</Button>
                            <Button variant="contained" onClick={async () => { await cinemaStore.deleteSession(session.id) ; }} color="error">Delete</Button>  
                        </Box>
                    )}
                </Box>
            </Box>
            <CustomSnackbar
                message="Session deleted"
                open={cinemaStore.getDeleteSnack()}
                autoHideDuration={3000}
                key="DeleteSnackBar"
                severity="error"
                onClose={() => cinemaStore.setDeleteSnack(false)}
            />
            <PurchaseTicketModal open={openModal} onClose={() => { setOpenModal(false); }} session={session} /> 
            <CustomSnackbar
                message="Ticket has been bought"
                open={userStore.getPurchaseSnack()}
                onClose={() => userStore.setPurchaseSnack(false)}
                severity="success"
            />
        </>
    );
})
