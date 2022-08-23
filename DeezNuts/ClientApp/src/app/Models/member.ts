import { Photo } from "./photo";

export interface Member {
  id: number;
  username: string;
  photoURL: string;
  knownAs: string;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  city: string;
  country: string;
  age: number;
  dateCreated: Date;
  lastActive: Date;
  photos: Photo[];
}
