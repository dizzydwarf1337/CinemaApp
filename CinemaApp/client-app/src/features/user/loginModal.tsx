import { Box, FormControl, Modal, Typography, TextField, Button } from "@mui/material";
import { useState } from "react";
import { useStore } from "../../assets/stores/store";
import loginModel from "../../assets/models/loginModel";

interface Props {
    open: boolean;
    onClose: () => void;
}

export default function PurchaseTicketModal({ open, onClose }: Props) {
    const { userStore } = useStore();
    const [ login, setLogin ] = useState<loginModel>({
        email: "",
        password:"",
    })
    const resetForm = () => {
        setLogin({ email: "", password: "" });
    }
    const handleClose = () => {
        resetForm();
        onClose();
    };
    const handleConfirm = async () => {
        userStore.logIn(login);
        onClose();
    }
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
            <Box display="flex" justifyContent="center">
                <Typography variant="h4" color="primary">
                    Login
                </Typography>
                </Box>
                <Box display="flex" flexDirection="column" gap="20px">
                    <FormControl fullWidth>
                        <TextField
                            type="email"
                            value={login.email}
                            onChange={(e) => setLogin({ ...login, email: e.target.value })}
                            label="Email"
                            required />    
                    </FormControl>
                    <FormControl>
                        <TextField
                            type="password"
                            value={login.password}
                            onChange={(e) => setLogin({ ...login, password: e.target.value })}
                            label="Password"
                            required />    
                    </FormControl>
                    <Button onClick={handleConfirm} variant="contained" color="success">Log in</Button>
                    <Button onClick={handleClose} variant="contained" color="error">Cancel</Button>
                </Box>
            </Box>
        </Modal>
    );
}
