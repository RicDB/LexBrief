// document.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommentsListService {
  constructor(private http: HttpClient) {
  }

  getComments() {
    return this.http.get<CommentDto[]>('/api/comments');
  }

  getCommentsSummary(comments: CommentDto[]) {
    return this.http.post<CommentsSummaryDto>('/api/comments/summary', comments);
  }

  addComment(comment: CommentDto) {
    return this.http.post<CommentDto>('/api/comments', comment);
  }
}
export interface CommentDto {
  id?: number;
  content?: string;
  sentiment?: number;
  ownerInitials?: string;
}

export interface CommentsSummaryDto {
  content: string;
  positive: string;
  neutral: string;
  negative: string;
}
