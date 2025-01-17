/**
 * Traffic Court Online Citizen Api
 * An API for creating violation ticket disputes
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { Field } from './field.model';


/**
 * A model representation of the extracted OCR results.
 */
export interface OcrViolationTicket { 
    /**
     * Gets or sets the saved image filename.
     */
    imageFilename?: string | null;
    /**
     * A global confidence of correctly extracting the document. This value will be low if the title of this   Violation Ticket form is not found (or of low confidence itself) or if the main ticket number is missing or invalid.
     */
    globalConfidence?: number;
    /**
     * An enumeration of all fields in this Violation Ticket.
     */
    fields?: { [key: string]: Field; } | null;
}

