import { Box, Typography } from "@mui/material";
import { observer } from "mobx-react-lite";
import { useStore } from "../../assets/stores/store";
import TicketListItem from "./ticketListItem";
import CustomSnackbar from "../../assets/layout/CustomSnackBar";


export default observer(function TicketsDashboard() {

    const { cinemaStore, userStore } = useStore();

    return (
        <>
            {userStore.tickets.length > 0 ? (
                <Box display="flex" flexDirection="column" gap="20px" m="100px">
                    <Box display="flex" flexDirection="column">
                        <Box>
                            <Typography variant="h2" color="primary">Future Sessions</Typography>
                        </Box>
                        <Box>
                            {userStore.tickets.filter(ticket => { const session = cinemaStore.sessions.find(x => x.id === ticket.sessionId); if (session) return new Date(session.date) > new Date(); else return false; })
                                .map(ticket => (
                                    <TicketListItem ticket={ticket} key={ticket.id} />

                                ))}
                        </Box>
                    </Box>
                    <Box display="flex" flexDirection="column">
                        <Box>
                            <Typography variant="h2" color="primary">Previous Sessions</Typography>
                        </Box>
                        <Box>
                            {userStore.tickets.filter(ticket => { const session = cinemaStore.sessions.find(x => x.id === ticket.sessionId); if (session) return new Date(session.date) < new Date(); else return false; })
                                .map(ticket => (
                                    <TicketListItem ticket={ticket} key={ticket.id} />

                                ))}
                        </Box>
                    </Box>
                </Box>) : (
                    <Box display="flex" flexDirection="column" gap="20px" m="100px">
                        <Typography variant="h2" color="error">No tickets found</Typography>
                    </Box>
                )
            }
            <CustomSnackbar
                message="Ticket refunded"
                key="TicketRefund"
                open={userStore.getRefundSnack()}
                onClose={() => userStore.setRefundSnack(false)}
                severity="error"
            />
        </>
    )
})