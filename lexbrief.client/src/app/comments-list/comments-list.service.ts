// document.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommentsListService {
  constructor(private http: HttpClient) {
  }

  getComments() {
    return this.http.get<CommentDto[]>('/api/comments');
  }
}

export interface CommentDto {
  id: number;
  content: string;
  sentiment: number;
  ownerInitials: string;
}
