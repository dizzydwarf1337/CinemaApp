import { Box, FormControl, Modal, Typography, TextField, Button } from "@mui/material";
import { useState } from "react";
import { useStore } from "../../assets/stores/store";
import user from "../../assets/models/user";

interface Props {
    open: boolean;
    onClose: () => void;
}

export default function RegisterModal({ open, onClose }: Props) {
    const { userStore } = useStore();
    const [register, setRegister] = useState<user>({
        email: "",
        password: "",
        role: "user",
        id: "00000000-0000-0000-0000-000000000000",
        name: "",
        lastName:""
    })
    const resetForm = () => {
        setRegister({ email: "", password: "", role: "user", id: "00000000-0000-0000-0000-000000000000", name: "", lastName: "" });
    }
    const handleClose = () => {
        resetForm();
        onClose();
    };
    const handleConfirm = async () => {
        userStore.createUser(register);
        resetForm();
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
                        Register
                    </Typography>
                </Box>
                <Box display="flex" flexDirection="column" gap="20px">
                    <FormControl fullWidth>
                        <TextField
                            type="email"
                            value={register.email}
                            onChange={(e) => setRegister({ ...register, email: e.target.value })}
                            label="Email"
                            required />
                    </FormControl>
                    <FormControl>
                        <TextField
                            type="password"
                            value={register.password}
                            onChange={(e) => setRegister({ ...register, password: e.target.value })}
                            label="Password"
                            required />
                    </FormControl>
                    <FormControl>
                        <TextField
                            type="text"
                            value={register.name}
                            onChange={(e) => setRegister({ ...register, name: e.target.value })}
                            label="First Name"
                            required />
                    </FormControl>
                    <FormControl>
                        <TextField
                            type="text"
                            value={register.lastName}
                            onChange={(e) => setRegister({ ...register, lastName: e.target.value })}
                            label="Last Name"
                            required />
                    </FormControl>
                    <Button onClick={handleConfirm} variant="contained" color="success">Register</Button>
                    <Button onClick={handleClose} variant="contained" color="error">Cancel</Button>
                </Box>
            </Box>
        </Modal>
    );
}
