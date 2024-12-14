import { Box, Button, FormControl, Modal, Typography, MenuItem, Select, InputLabel } from "@mui/material";
import { useState } from "react";
import Session from "../../assets/models/session";
import Ticket from "../../assets/models/ticket";
import { useStore } from "../../assets/stores/store";

interface Props {
    open: boolean;
    onClose: () => void;
    session: Session;
}

export default function PurchaseTicketModal({ open, onClose, session }: Props) {
    const { userStore } = useStore();
    const [ticket, setTicket] = useState<Ticket>({
        id: "00000000-0000-0000-0000-000000000000",
        sessionId: session.id,
        userId: userStore.user?.id!,
        seat: "",
        numberOfSeats: 1,
        price: session.ticketPrice,
        status: "Booked",
        created: new Date(),
    });

    const resetForm = () => {
        setSelectedSeats([""]);
        setTicket({
            id: "00000000-0000-0000-0000-000000000000",
            sessionId: session.id,
            userId: userStore.user?.id!,
            seat: "",
            numberOfSeats: 1,
            price: session.ticketPrice,
            status: "Booked",
            created: new Date(),
        });
    };

    const [selectedSeats, setSelectedSeats] = useState<string[]>([""]);
    const rows = ["A", "B", "C", "D", "E"];
    const seats = Array.from({ length: 10 }, (_, i) => i + 1);

    const handleRowChange = (index: number, value: string) => {
        const updatedSeats = [...selectedSeats];
        const seat = updatedSeats[index] || "";
        updatedSeats[index] = `${value}${seat.slice(1)}`;
        setSelectedSeats(updatedSeats);
    };

    const handleSeatChange = (index: number, value: string) => {
        const updatedSeats = [...selectedSeats];
        const seat = updatedSeats[index] || "";
        updatedSeats[index] = `${seat[0] || ""}${value}`;
        setSelectedSeats(updatedSeats);
    };

    const handleAddSeat = () => {
        setSelectedSeats([...selectedSeats, ""]);
    };

    const handleConfirm = async () => {
        const seatString = selectedSeats.join(",");
        setTicket({ ...ticket, seat: seatString });
        await userStore.addTicket({ ...ticket, seat: seatString });
        onClose();
    };

    const handleClose = () => {
        resetForm();
        onClose();
    };

    return (
        <Modal open={open} onClose={onClose}>
            <Box
                display="flex"
                flexDirection="column"
                gap={2}
                p={3}
                bgcolor="white"
                borderRadius={2}
                boxShadow={3}
                maxWidth={400}
                mx="auto"
                mt="10%"
            >
                <Typography variant="h4" color="Primary">
                    Ticket Details
                </Typography>
                <form onSubmit={(e) => { e.preventDefault(); handleConfirm(); }}>
                    <Box display="flex" flexDirection="column" gap={2}>
                        {selectedSeats.map((seat, index) => (
                            <Box key={index} display="flex" gap={2}>
                                <FormControl fullWidth>
                                    <InputLabel id={`row-select-label-${index}`}>Row</InputLabel>
                                    <Select
                                        labelId={`row-select-label-${index}`}
                                        value={seat[0] || ""}
                                        onChange={(event) => handleRowChange(index, event.target.value)}
                                        label="Row"
                                        required
                                    >
                                        {rows.map((row) => (
                                            <MenuItem key={row} value={row}>
                                                <Typography color="black">{row}</Typography>
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                                <FormControl fullWidth>
                                    <InputLabel id={`seat-select-label-${index}`}>Seat</InputLabel>
                                    <Select
                                        labelId={`seat-select-label-${index}`}
                                        value={seat.slice(1) || ""}
                                        onChange={(event) => handleSeatChange(index, event.target.value)}
                                        label="Seat"
                                        required
                                    >
                                        {seats.map((seatNumber) => (
                                            <MenuItem key={seatNumber} value={seatNumber}>
                                                <Typography color="black">{seatNumber}</Typography>
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Box>
                        ))}
                        <Box>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={handleAddSeat}
                                sx={{ mt: 2 }}
                                fullWidth
                            >
                                Add Seat
                            </Button>
                            <Box display="flex" justifyContent="space-between" mt={2}>
                                <Button variant="contained" color="error" onClick={handleClose}>
                                    Cancel
                                </Button>
                                <Button variant="contained" color="success" type="submit">
                                    Pay
                                </Button>
                            </Box>
                        </Box>
                    </Box>
                </form>
            </Box>
        </Modal>
    );
}
