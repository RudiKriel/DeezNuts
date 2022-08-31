export interface Message {
  id: number;
  content: string;
  senderId: number;
  senderUsername: string;
  senderPhotoUrl: string;
  recipientId: number;
  recipientUsername: string;
  recipientPhotoUrl: string;
  dateRead: Date;
  messageSent: Date;
}
