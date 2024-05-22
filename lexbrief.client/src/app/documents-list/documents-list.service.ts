// document.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DocumentsListService {
  constructor(private http: HttpClient) {
  }

  getDocuments() {
    return this.http.get<DocumentDto[]>('/api/document');   
  }

  getDocument(id: number) {
    return this.http.get<DocumentDetailDto>( `/api/document/${id}`);
  }
}

export interface DocumentDto {
  id: number;
  title: string;
  summary: string;
}

export interface DocumentDetailDto extends DocumentDto {
  content: string;
}
