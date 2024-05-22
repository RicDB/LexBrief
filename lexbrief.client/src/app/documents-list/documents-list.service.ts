// document.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DocumentsListService {
  constructor(private http: HttpClient) {
  }

  getDocuments() {
    return this.http.get<DocumentDto[]>('/api/document');   
  }
}

export interface DocumentDto {
  id: number;
  title: string;
}
