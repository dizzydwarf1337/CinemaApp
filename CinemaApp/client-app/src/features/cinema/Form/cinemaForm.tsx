import { observer } from "mobx-react-lite";
import { useStore } from "../../../assets/stores/store";
import { Form, useParams } from "react-router";
import Cinema from "../../../assets/models/cinema";
import { ChangeEvent, useEffect, useState } from "react";
import { Box, Button, FormControl, Input, TextField } from "@mui/material";
import { useNavigate } from "react-router";

export default observer(function CinemaForm() {
    const { cinemaStore } = useStore();
    const { id } = useParams();
    const navigate = useNavigate();
    const [cinemaData, setCinemaData] = useState<Cinema>({
        id: "00000000-0000-0000-0000-000000000000",
        name: "",
        address: "",
        imagePath: "",
    });
    
    const [photo, setPhoto] = useState<File>();
    const [numOfHalls, setHalls] = useState<Number>(0);
    useEffect(() => {
        if (id) {
            const cinema = cinemaStore.cinemas.find((cinema) => cinema.id === id);
            if (cinema) setCinemaData(cinema);
        }
    }, [id, cinemaStore]);

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setCinemaData({ ...cinemaData, [name]: value });
    };
    const handleHallChange = (event: ChangeEvent<HTMLInputElement>) => {
        setHalls(Number(event.target.value));
    };
    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setPhoto(event.target.files[0]);
        }
    };

    const handleSubmit = (event: ChangeEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (id) {
            cinemaStore.updateCinema(cinemaData, photo);
        } else {
            cinemaStore.createCinema(cinemaData, numOfHalls, photo);
        }
        navigate('/cinema');
    };

    return (
        <Box display="flex" flexDirection="column" bgcolor="" boxShadow="2px 2px 2px 2px grey" width="400px" height="500px" m="100px" borderRadius="10px">
            <Form onSubmit={handleSubmit}>
                <Box display="flex" flexDirection="column" m="20px" gap="20px" height="100%" justifyContent="space-between">
                    <FormControl sx={{color:"black"} }>
                        <TextField
                            value={cinemaData.name}
                            name="name"
                            onChange={handleInputChange}
                            label="Cinema Name"
                        />
                    </FormControl>
                    <FormControl>
                        <TextField
                            value={cinemaData.address}
                            name="address"
                            onChange={handleInputChange}
                            label="Address"
                        />
                    </FormControl>
                    {!id? (
                        <FormControl>
                            <TextField
                                type="number"
                                value={numOfHalls}
                                onChange={handleHallChange}
                                name="numOfHalls"
                                label="Number Of Halls"
                            />
                        </FormControl>
                        ) : (null)
                    }
                    <FormControl>
                        <Input
                            type="file"
                            onChange={handleFileChange}
                            name="imagePath"
                        />
                    </FormControl>
                    
                    <FormControl>
                        <Button type="submit" variant="contained"  color={id ? "warning" : "success"}>
                            {id ? "Update Cinema" : "Create Cinema"}
                        </Button>
                    </FormControl>
                </Box>
            </Form>
        </Box>
    );
});
