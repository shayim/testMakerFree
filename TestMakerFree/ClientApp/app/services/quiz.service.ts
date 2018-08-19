import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IQuiz } from '../Interfaces/quiz';
import { IQuestion } from '../Interfaces/question';
import { IResult } from '../Interfaces/result';
import { IAnswer } from '../Interfaces/answer';

@Injectable({ providedIn: 'root' })
export class QuizService {
    private apiUrl = 'api/quiz';
    constructor(@Inject('BASE_URL') private baseUrl: string, private http: HttpClient) { }

    getQuizzes(type: string, itemNum: number): Observable<IQuiz[]> {
        const url = `${this.baseUrl}/${this.apiUrl}/${type}/${itemNum}`;
        return this.http.get<IQuiz[]>(url);
    }

    getQuizById(id: number | string): Observable<IQuiz> {
        const url = `${this.baseUrl}/${this.apiUrl}/${id}`;
        return this.http.get<IQuiz>(url);
    }

    postOrUpdate(quiz: IQuiz): Observable<IQuiz> {
        const url = `${this.baseUrl}/${this.apiUrl}`;
        if (quiz.id) {
            return this.http.put<IQuiz>(url, quiz);
        } else {
            return this.http.post<IQuiz>(url, quiz);
        }
    }

    remove(id: number, userId: string): Observable<any> {
        const url = `${this.baseUrl}/${this.apiUrl}/${id}?userId=${userId}`;
        return this.http.delete(url);
    }

    getQuestions(quizId: number): Observable<IQuestion[]> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/question`;

        return this.http.get<IQuestion[]>(url);
    }

    getQuestion(quizId: number, id: number): Observable<IQuestion> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/question/${id}`;
        return this.http.get<IQuestion>(url);
    }

    postOrUpdateQuestion(question: IQuestion): Observable<IQuestion> {
        const url = `${this.baseUrl}/${this.apiUrl}/${question.quizId}/question`;
        if (question.id) {
            return this.http.put<IQuestion>(url, question);
        } else {
            return this.http.post<IQuestion>(url, question);
        }
    }

    removeQuestion(quizId: number, questionId: number): Observable<any> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/question/${questionId}`;
        return this.http.delete(url);
    }

    removeResult(quizId: number, resultId: number): Observable<any> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/result/${resultId}`;
        return this.http.delete(url);
    }

    getResult(quizId: number, id: number): Observable<IResult> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/result/${id}`;
        return this.http.get<IResult>(url);
    }

    getResults(quizId: number): Observable<IResult[]> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/result`;

        return this.http.get<IResult[]>(url);
    }

    postOrUpdateResult(result: IResult): Observable<IResult> {
        const url = `${this.baseUrl}/${this.apiUrl}/${result.quizId}/result`;
        if (result.id) {
            return this.http.put<IResult>(url, result);
        } else {
            return this.http.post<IResult>(url, result);
        }
    }

    getAnswers(quizId: number, questionId: number): Observable<IAnswer[]> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/question/${questionId}/answer`;

        return this.http.get<IAnswer[]>(url);
    }

    getAnswer(quizId: number, questionId: number, anwserId: number): Observable<IAnswer> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/question/${questionId}/answer/${anwserId}`;
        return this.http.get<IAnswer>(url);
    }

    postOrUpdateAnswer(quizId: number, questionId: number, answer: IAnswer) : Observable<IAnswer>{
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/question/${questionId}/answer`;
        if (answer.id) {
            return this.http.put<IAnswer>(url, answer);
        } else {
            return this.http.post<IAnswer>(url, answer);
        }
    }

    removeAnswer(quizId: number, questionId: number, anwserId: number): Observable<any> {
        const url = `${this.baseUrl}/${this.apiUrl}/${quizId}/question/${questionId}/answer/${anwserId}`;
        return this.http.delete(url);
    }
}