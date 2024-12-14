import { Box, Button, Typography } from "@mui/material";
import Ticket from "../../assets/models/ticket";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router";
import UserStore from "../../assets/stores/userStore";
import CustomSnackbar from "../../assets/layout/CustomSnackBar";


interface Props {
    ticket: Ticket;
}
export default observer(function TicketListItem({ticket}:Props) {
    const { cinemaStore, userStore } = useStore();
    const session = cinemaStore.sessions.find(session => session.id === ticket.sessionId);
    const movie = cinemaStore.movies.find(movie => movie.id === session?.movieId);
    const cinema = cinemaStore.cinemas.find(cinema => cinema.id === cinemaStore.halls.find(hall => hall.id == session?.hallId)?.cinemaId);
    console.log(userStore.getRefundSnack());
    return (
        <>
            <Box display="flex" flexDirection="column" sx={{ backgroundColor: "white", m: "7px", p: "10px", borderRadius: "17px", boxShadow: "2px 2px 2px grey" }}>
                <Box display="flex" flexDirection="row" alignItems="center" gap="50px">
                    <Link to={`/movie/${movie?.id}`} style={{ flex:"0.5" } }>
                        <Typography variant="h4" color="secondary" sx={{ textShadow: "1px 1px 1px #1a202c" }} >{movie?.title}</Typography>
                    </Link>
                    <Link to={`/cinema/${cinema?.id}`} style={{ flex:"1" }} >
                    <Typography variant="h4" color="primary">{cinema?.name}</Typography>
                    </Link>
                </Box>
                <Box display="flex" justifyContent="space-between" alignItems="center" >
                    <Box display="flex" flexDirection="row" gap="20px" >
                        <Typography>Date {new Date(session?.date).toLocaleDateString()} at {new Date(session?.date).toLocaleTimeString()}</Typography>
                        <Typography>Seat: {ticket.seat}</Typography>
                        <Typography>Number of Seats: {ticket.numberOfSeats}</Typography>
                        <Typography>Price: {ticket.price}</Typography>
                        <Typography>Bought: {new Date(ticket.created).toLocaleDateString()}</Typography>
                        <Typography>Status: {ticket.status}</Typography>
                    </Box>
                    <Box display="flex">
                        <Button variant="contained" color="error" size="small" onClick={() => {userStore.deleteTicket(ticket.id) }}>Return Ticket</Button>
                        </Box>
                </Box>
            </Box>

                
        </>
    )
})