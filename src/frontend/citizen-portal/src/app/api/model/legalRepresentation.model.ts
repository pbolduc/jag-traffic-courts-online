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


export interface LegalRepresentation { 
    /**
     * Name of the law firm that will represent the disputant at the hearing.
     */
    law_firm_name?: string | null;
    /**
     * Full name of the lawyer who will represent the disputant at the hearing.
     */
    lawyer_full_name?: string | null;
    /**
     * Email address of the lawyer who will represent the disputant at the hearing.
     */
    lawyer_email?: string | null;
    /**
     * Address of the lawyer who will represent the disputant at the hearing.
     */
    lawyer_address?: string | null;
    /**
     * Address of the lawyer who will represent the disputant at the hearing.
     */
    lawyer_phone_number?: string | null;
}

