/**
 * VTC Staff API
 * Violation Ticket Centre Staff API
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { DisputedCountPlea } from './disputedCountPlea.model';


export interface DisputedCount { 
    createdBy?: string | null;
    createdTs?: string;
    modifiedBy?: string | null;
    modifiedTs?: string;
    /**
     * ID
     */
    id?: string;
    plea?: DisputedCountPlea;
    count?: number;
    requestTimeToPay?: boolean;
    requestReduction?: boolean;
    appearInCourt?: boolean;
    additionalProperties?: { [key: string]: any; } | null;
}

