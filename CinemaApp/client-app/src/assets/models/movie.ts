
export default interface Movie {
    id: string,
    title: string,
    description: string,
    genre: string,
    director: string,
    duration: Date |string,
    imagePath?: string,
}