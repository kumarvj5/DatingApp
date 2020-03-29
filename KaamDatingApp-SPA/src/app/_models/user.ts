import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    knownAs: string;
    gender: string;
    age: number;
    created: Date;
    lastActive: any;
    photoUrl: string;
    city: string;
    country: string;
    introduction?: string;
    interests?: string;
    lookingFor?: string;
    photos?: Photo[];
}
